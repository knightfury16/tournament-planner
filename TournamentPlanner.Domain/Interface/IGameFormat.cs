using TournamentPlanner.Domain.Entities;
using MatchType = TournamentPlanner.Domain.Entities.MatchType;

namespace TournamentPlanner.Domain.Interface;

public interface IGameFormat
{
    IScore CreateInitialScore();
    bool IsValidScore(IScore score);
    abstract Player DetermineWinner(Player player1, Player player2, IScore score);
    string SerializeScore(object score);
    List<PlayerStanding> GetGroupStanding(Tournament tournament, MatchType matchType, bool completeStanding = false);
    IScore DeserializeScore(object scoreData);
}
