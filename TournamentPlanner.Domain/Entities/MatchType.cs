using TournamentPlanner.Domain.Common;
using TournamentPlanner.Domain.Interface;

namespace TournamentPlanner.Domain.Entities
{
    public class MatchType: BaseEntity  
    {
        public required string Name { get; set; }
        public List<Player> Players { get; set; } = new List<Player>();
        public List<Match> Matches { get; set; } = new List<Match>();
    }
}