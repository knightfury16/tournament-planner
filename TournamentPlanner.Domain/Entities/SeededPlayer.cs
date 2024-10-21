using TournamentPlanner.Domain.Common;

namespace TournamentPlanner.Domain.Entities;

public class SeededPlayer : BaseEntity
{
    public Player? Player { get; set; }
    public int PlayerId { get; set; }
    public MatchType? MatchType { get; set; }
    public int MatchTypeId { get; set; }

}
