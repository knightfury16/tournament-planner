using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.GameTypeHandler;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Enum;

namespace TournamentPlanner.Application.Common;

public class GameFormatFactory : IGameFormatFactory
{
    public GameFormat GetGameFormat(GameTypeSupported gameType)
    {
        return gameType switch
        {
            GameTypeSupported.TableTennis => new TableTennisGameFormat(),
            GameTypeSupported.EightBallPool => new EightBallPoolGameFormat(),
            _ => throw new ArgumentException($"{gameType} not supported yet"),
        };
    }
}
