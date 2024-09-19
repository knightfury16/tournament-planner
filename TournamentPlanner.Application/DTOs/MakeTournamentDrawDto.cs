namespace TournamentPlanner.Application.DTOs;

public class MakeTournamentDrawDto
{
    public List<int>? SeedersId { get; set; }
    public string? MatchTypePrefix { get; set; }
}
