using System.ComponentModel.DataAnnotations;

namespace AprobadorIdeas.Models
{
    public class Team
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [Required]
        [Range(1, 2, ErrorMessage = "El número de integrantes no está permitido.")]
        public int MembersCount { get; set; }

        [Required]
        public string Member1Name { get; set; } = null!;

        public string? Member2Name { get; set; }
    }
}
