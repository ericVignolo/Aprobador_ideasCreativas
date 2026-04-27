using Microsoft.AspNetCore.Mvc;
using AprobadorIdeas.Data;
using AprobadorIdeas.Models;
using Microsoft.EntityFrameworkCore;

namespace AprobadorIdeas.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Obtener únicamente las ideas aprobadas de todos los equipos para la cartelera pública.
            var approvedIdeas = await _context.Ideas
                .Include(i => i.Team)
                .Where(i => i.Status == IdeaStatus.Approved)
                .OrderByDescending(i => i.SubmissionDateTime)
                .ToListAsync();

            return View(approvedIdeas);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Teams.AnyAsync(t => t.Name == model.TeamName))
                {
                    ModelState.AddModelError("TeamName", "ya hay un equipo con ese nombre, ingrese otro nombre");
                    return View(model);
                }

                var team = new Team
                {
                    Name = model.TeamName,
                    Password = model.Password,
                    MembersCount = model.MembersCount,
                    Member1Name = model.Member1Name,
                    Member2Name = model.Member2Name
                };

                _context.Teams.Add(team);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Ingresado con exito";
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult SubmitIdea()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SubmitIdea(SubmitIdeaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var team = await _context.Teams.FirstOrDefaultAsync(t => t.Name == model.TeamName && t.Password == model.Password);

                if (team == null)
                {
                    ModelState.AddModelError("", "Credenciales de equipo inválidas.");
                    return View(model);
                }

                var idea = new Idea
                {
                    TeamId = team.Id,
                    Description = model.IdeaText,
                    SubmissionDateTime = DateTime.Now,
                    IsCreative = false,
                    IsWellProposed = false,
                    Status = IdeaStatus.Pending
                };

                _context.Ideas.Add(idea);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "la idea fue postulada con éxito, espere por aprobación.";
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult CheckMyIdeas()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CheckMyIdeas(CheckIdeasViewModel model)
        {
            if (ModelState.IsValid)
            {
                var team = await _context.Teams.FirstOrDefaultAsync(t => t.Name == model.TeamName && t.Password == model.Password);

                if (team == null)
                {
                    ModelState.AddModelError("", "Credenciales de equipo inválidas.");
                    return View(model);
                }

                // Si son válidas, mostramos sus ideas en otra vista temporal
                var myIdeas = await _context.Ideas
                    .Where(i => i.TeamId == team.Id)
                    .OrderByDescending(i => i.SubmissionDateTime)
                    .ToListAsync();
                
                ViewBag.TeamName = team.Name;

                return View("MyIdeas", myIdeas);
            }

            return View(model);
        }
    }
}
