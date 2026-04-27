using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using AprobadorIdeas.Data;
using AprobadorIdeas.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace AprobadorIdeas.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly TextAnalyzer.Services.SimilarityEngine _similarityEngine;

        // Hardcoded credentials as simple requirement
        private const string AdminUser = "profesor";
        private const string AdminPass = "profesor123";

        public AdminController(ApplicationDbContext context, TextAnalyzer.Services.SimilarityEngine similarityEngine)
        {
            _context = context;
            _similarityEngine = similarityEngine;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (username == AdminUser && password == AdminPass)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Dashboard");
            }

            ModelState.AddModelError("", "Credenciales incorrectas");
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            // Get ideas ordered by date and eager-load team names
            var ideas = await _context.Ideas
                .Include(i => i.Team)
                .OrderByDescending(i => i.SubmissionDateTime)
                .ToListAsync();

            return View(ideas);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CompareIdeas(double threshold = 0.20)
        {
            var allIdeas = await _context.Ideas
                .Include(i => i.Team)
                .Where(i => i.Description != null && i.Description.Trim().Length > 10) // Evitar nulos o vacíos
                .ToListAsync();

            var input = allIdeas.Select(i => (
                Id: i.Id, 
                Text: i.Description, 
                TeamName: i.Team?.Name ?? "Desconocido"
            )).ToList();

            var similarPairs = _similarityEngine.FindSimilarIdeas(input, threshold);

            ViewBag.Threshold = threshold;
            return View(similarPairs);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ReviewIdea(int id, bool isCreative, bool isWellProposed, IdeaStatus status, string? professorFeedback)
        {
            var idea = await _context.Ideas.FindAsync(id);
            if (idea == null)
            {
                return NotFound();
            }

            idea.IsCreative = isCreative;
            idea.IsWellProposed = isWellProposed;
            idea.Status = status;
            idea.ProfessorFeedback = professorFeedback;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = status switch
            {
                IdeaStatus.Approved => "Idea aprobada con éxito",
                IdeaStatus.Rejected => "Idea desaprobada",
                IdeaStatus.OnTrack => "Feedback enviado, idea encaminada",
                _ => "Idea actualizada"
            };

            return RedirectToAction("Dashboard");
        }
    }
}
