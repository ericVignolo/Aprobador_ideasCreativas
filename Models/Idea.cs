using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AprobadorIdeas.Models
{
    public enum IdeaStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2,
        OnTrack = 3
    }

    public class Idea
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TeamId { get; set; }
        
        [ForeignKey(nameof(TeamId))]
        public Team? Team { get; set; }

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public DateTime SubmissionDateTime { get; set; }

        public bool IsCreative { get; set; }
        public bool IsWellProposed { get; set; }
        
        // El estado definitivo de la idea
        public IdeaStatus Status { get; set; }

        // El feedback o nota que el profesor puede dejar al estudiante
        public string? ProfessorFeedback { get; set; }
    }
}
