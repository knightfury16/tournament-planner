using System.ComponentModel.DataAnnotations;

namespace TournamentPlanner.Application.DTOs
{
    public class AddAdminDto
    {

        [Required]
        [MinLength(5, ErrorMessage = "Name should at least 5 character long")]
        [MaxLength(100, ErrorMessage = "Name should at most 100 charecter lond")]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }


        [Required]
        [Phone]
        public required string PhoneNumber { get; set; }

    }
}