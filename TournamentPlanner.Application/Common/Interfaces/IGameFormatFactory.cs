using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Enum;
using TournamentPlanner.Domain.Interface;

namespace TournamentPlanner.Application.Common.Interfaces;

public interface IGameFormatFactory
{
    public GameFormat GetGameFormat(GameTypeSupported gameType);
}
