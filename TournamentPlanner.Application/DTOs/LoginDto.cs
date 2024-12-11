using System.ComponentModel.DataAnnotations;

namespace TournamentPlanner.Application;

public class LoginDto
{
  [Required]
  [EmailAddress]
  public required string Email { get; set; }

  [Required]
  [DataType(DataType.Password)]
  public required string Password { get; set; }

}
