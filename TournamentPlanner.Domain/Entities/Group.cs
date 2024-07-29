using TournamentPlanner.Domain.Interface;

namespace TournamentPlanner.Domain.Entities
{

    public class Group<TScore>: MatchType<TScore> where TScore : IScore
    {
    }
}