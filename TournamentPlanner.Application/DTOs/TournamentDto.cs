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
        public DateTime? RegistrationLastDate { get; set; }
        public string? Venue { get; set; }
        public decimal RegistrationFee { get; set; }
        public int MinimumAgeOfRegistration { get; set; }
        public int KnockOutStartNumber { get; set; }
        public string? Status { get; set; }
        public int MaxParticipant { get; set; } // Default: 26 Group (A-Z) * 4 per group = 104
        public GameTypeDto? GameTypeDto { get; set; }
        public List<PlayerDto>? Participants { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public string? TournamentType { get; set; }
    }
}
