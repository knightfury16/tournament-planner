namespace TournamentPlanner.Domain.Entities;

using TournamentPlanner.Domain.Common;

public abstract class MatchType : BaseEntity
{
    public required string Name { get; set; } //group name, eg. GroupA or Elimination1
    public List<Player> Players { get; set; } = new List<Player>();
    public List<Round> Rounds { get; set; } = new();
    public Draw? Draw { get; set; }
    public bool IsCompleted { get; set; }
}
