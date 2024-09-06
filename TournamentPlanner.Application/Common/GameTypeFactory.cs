using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.GameTypeHandler;
using TournamentPlanner.Domain.Enum;

namespace TournamentPlanner.Application.Common;

public class GameTypeFactory : IGameTypeFactory
{
    public IGameTypeHandler GetTheGameTypeHandler(GameTypeSupported gameType)
    {
        //Iterate over all the game type supoported enum and do a switch on those value
        return gameType switch
        {
            GameTypeSupported.TableTennis => new TableTennisGameTypeHandler(),
            _ => throw new ArgumentException($"Unsupported game type {nameof(gameType)}")
        };
    }
}
