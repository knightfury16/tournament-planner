using TournamentPlanner.Domain.Common;

namespace TournamentPlanner.Domain.Entities;

//draw as in lottery draw, here being used as Group draw
public class Draw: BaseEntity
{
    public required Tournament Tournament { get; set; }
    public int TournamentId { get; set; }

    public required MatchType MatchType { get; set; }
    public int MatchTypeId { get; set; }
    
}
