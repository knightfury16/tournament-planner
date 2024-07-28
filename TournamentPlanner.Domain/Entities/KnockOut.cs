using TournamentPlanner.Domain.Common;
using TournamentPlanner.Domain.Interface;

namespace TournamentPlanner.Domain.Entities
{
    public class KnockOut<TScore>: BaseEntity where TScore : IScore
    {
        public  required string Name { get; set; }
        public List<Player> Players { get; set; } = new List<Player>();
        public List<Match<TScore>> Matches { get; set; } = new List<Match<TScore>>();
    }
}