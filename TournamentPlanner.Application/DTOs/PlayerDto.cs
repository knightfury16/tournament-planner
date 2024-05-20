using System.ComponentModel.DataAnnotations;
using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.Application.DTOs
{
    public class PlayerDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Phone]
        [MaxLength(12)]
        public string? PhoneNumber { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public int? TournamentId { get; set; }
    }
}
