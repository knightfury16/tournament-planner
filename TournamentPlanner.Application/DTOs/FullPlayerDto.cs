using System.ComponentModel.DataAnnotations;

namespace TournamentPlanner.Application.DTOs
{
    public class FullPlayerDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public int? Age { get; set; }

        public int Weight { get; set; }

        public List<TournamentDto>? Tournaments { get; set; }
        public List<GameStatisticDto>? GameStatistics { get; set; }
    }
}

