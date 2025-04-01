namespace TournamentPlanner.Application.DTOs;

public class GameStatisticDto
{
    public PlayerDto? Player { get; set; }
    public required GameTypeDto GameType { get; set; }
    public int GamePlayed { get; set; }
    public int GameWon { get; set; }
    public double WinRatio => GamePlayed > 0 ? (double)GameWon / GamePlayed : 0;
}
