using System.ComponentModel.DataAnnotations;
using TournamentPlanner.Domain.Enum;

namespace TournamentPlanner.Application.DTOs
{
    public class TournamentDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }

    }
}
