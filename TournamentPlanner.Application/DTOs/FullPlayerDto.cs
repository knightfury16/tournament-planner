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
        public int GamePlayed { get; set; }
        public int GameWon { get; set; }

        public double WinRatio { get; set; }

    }
}