namespace TournamentPlanner.Application.DTOs;

public class ApplicationUserDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public string? UserName { get; set; }
    public string? PhoneNumber { get; set; }
    public int DomainUserId { get; set; }
}
