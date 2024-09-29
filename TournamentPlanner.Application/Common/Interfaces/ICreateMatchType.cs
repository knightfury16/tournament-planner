using TournamentPlanner.Domain.Entities;
using MatchType = TournamentPlanner.Domain.Entities.MatchType;

namespace TournamentPlanner.Application.Common.Interfaces;

public interface ICreateMatchType
{
    //SeederPlayes list is a validated players list of a specific tournament 
    public Task<IEnumerable<MatchType>?> CreateMatchType(Tournament tournament, List<Player> players, string? prefix = null, List<int>? seederPlayerIds = null); //im explicitly specifiying players that will play
}
