using System.ComponentModel.DataAnnotations;

namespace TournamentPlanner.Application.DTOs
{
    public class AddPlayerDto
    {
        [Required]
        [MinLength(5, ErrorMessage = "Name should at least 5 character long")]
        [MaxLength(100, ErrorMessage = "Name should at most 100 charecter lond")]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid age")]
        public int Age { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Please enter valid weight")]
        public int Weight { get; set; }

    }
}