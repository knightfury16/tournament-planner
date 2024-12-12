namespace TournamentPlanner.Application.Common.Interfaces;

public interface ICurrentUser
{
    public string? Name { get; }
    public string? Email { get; }
    public int? DomainUserId { get; }
    public string? Role { get; }
}
