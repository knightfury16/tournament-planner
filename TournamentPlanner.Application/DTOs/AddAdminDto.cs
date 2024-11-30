using System.ComponentModel.DataAnnotations;

namespace TournamentPlanner.Application.DTOs
{
    public class AddAdminDto
    {

        [Required]
        [MinLength(5, ErrorMessage = "Name should at least 5 character long")]
        [MaxLength(100, ErrorMessage = "Name should at most 100 charecter long")]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Name should at least 6 character long")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*\W).{6,}$", ErrorMessage = "Passwords must have at least one non-alphanumeric character, one digit ('0'-'9'), and one uppercase letter ('A'-'Z').")]
        public required string Password { get; set; }


        [Required]
        [Phone]
        public required string PhoneNumber { get; set; }

    }
}