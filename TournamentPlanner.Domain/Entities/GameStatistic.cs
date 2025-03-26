using TournamentPlanner.Domain.Common;

namespace TournamentPlanner.Domain.Entities;

public class GameStatistic : BaseEntity
{
    public required Player Player { get; set; }
    public int PlayerId { get; set; }
    public required GameType GameType { get; set; }
    public int GameTypeId { get; set; }
    public int GamePlayed { get; set; }
    public int GameWon { get; set; }
    public double WinRatio => GamePlayed > 0 ? (double)GameWon / GamePlayed : 0;
}
