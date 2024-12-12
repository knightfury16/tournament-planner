using System.Security.Claims;

namespace TournamentPlanner.Application.DTOs;

public class UserInfoDto
{
    public required string Email { get; set; }
    public required string Name { get; set; }
    public string? Role { get; set; }
    public List<Claim> UserClaims { get; set; } = new();
}
