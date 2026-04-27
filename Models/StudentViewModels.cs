using System.ComponentModel.DataAnnotations;

namespace AprobadorIdeas.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "El nombre de equipo es obligatorio")]
        public string TeamName { get; set; } = null!;

        [Required(ErrorMessage = "El password es obligatorio")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "La confirmación de password es obligatoria")]
        [Compare("Password", ErrorMessage = "Los passwords no coinciden")]
        public string ConfirmPassword { get; set; } = null!;

        [Required(ErrorMessage = "El número de integrantes es obligatorio")]
        [Range(1, 2, ErrorMessage = "El numero de integrantes no esta permitido")]
        public int MembersCount { get; set; }

        [Required(ErrorMessage = "El nombre del integrante 1 es obligatorio")]
        public string Member1Name { get; set; } = null!;

        public string? Member2Name { get; set; }
    }

    public class SubmitIdeaViewModel
    {
        [Required(ErrorMessage = "El nombre del equipo es obligatorio")]
        public string TeamName { get; set; } = null!;

        [Required(ErrorMessage = "El password del equipo es obligatorio")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "El texto de la idea es obligatorio")]
        public string IdeaText { get; set; } = null!;
    }

    public class CheckIdeasViewModel
    {
        [Required(ErrorMessage = "El nombre del equipo es obligatorio")]
        public string TeamName { get; set; } = null!;

        [Required(ErrorMessage = "El password del equipo es obligatorio")]
        public string Password { get; set; } = null!;
    }
}
