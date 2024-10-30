using TournamentPlanner.Application.DTOs;

namespace TournamentPlanner.Application;

public class PlayerStandingDto
{

    public required PlayerDto Player { get; set; }
    public int MatchPoints { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
    public int GamesWon { get; set; }
    public int GamesLost { get; set; }
    public int GameDifference { get => GamesWon - GamesLost; }
    public int PointsWon { get; set; }
    public int PointsLost { get; set; }
    public int PointsDifference { get => PointsWon - PointsLost; }
    public int Ranking { get; set; }
}
