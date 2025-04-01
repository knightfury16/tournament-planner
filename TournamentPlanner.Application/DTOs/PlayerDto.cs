using System.ComponentModel.DataAnnotations;

namespace TournamentPlanner.Application.DTOs
{
    public class PlayerDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public int? Age { get; set; }
    }
}
