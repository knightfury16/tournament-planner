using TournamentPlanner.Domain.Enum;

namespace TournamentPlanner.Application.Common.Interfaces;

public interface IGameTypeFactory
{
    public IGameTypeHandler GetTheGameTypeHandler(GameTypeSupported gameType);
}
