namespace TournamentPlanner.Domain.Entities;

using TournamentPlanner.Domain.Common;

public abstract class MatchType : BaseEntity
{
    public required string Name { get; set; }
    public List<Player> Players { get; set; } = new List<Player>();
    public List<Match> Matches { get; set; } = new();
    public required Tournament Tournament { get; set; }
    public int TournamentId { get; set; }
}
