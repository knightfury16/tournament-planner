using TournamentPlanner.Domain.Common;
using TournamentPlanner.Domain.Interface;

namespace TournamentPlanner.Domain.Entities
{
    public class KnockOut : MatchType
    {
        public int Round { get; set; }
    }
}