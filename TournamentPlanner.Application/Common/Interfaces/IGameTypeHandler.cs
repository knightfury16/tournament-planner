using TournamentPlanner.Application.GameTypeHandler;

namespace TournamentPlanner.Application.Common.Interfaces;
public interface IGameTypeHandler
{
    bool ValidateGameSpecificData(string gameSpecificScore);
    IScore DeserializeScore(string scoreData);
    string SerializeScore(IScore score);
}