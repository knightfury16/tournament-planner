using System.ComponentModel.DataAnnotations;
using TournamentPlanner.Domain.Entities;
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
        public TournamentStatus? Status { get; set; }
        public GameType? GameType { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }

    }
}
