using TournamentPlanner.Domain.Common;
using TournamentPlanner.Domain.Interface;

namespace TournamentPlanner.Domain.Entities
{
    public class KnockOut<TScore> : MatchType<TScore> where TScore : IScore
    {
        public int Round { get; set; }
    }
}