using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.Domain;

//A Basic model, complex calculation can inherit and modify it
public class PlayerStanding
{
    public required Player Player { get; set; }
    public int MatchPoints { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
    public int GamesWon { get; set; }
    public int GamesLost { get; set; }
    public int GameDifference { get; set; }
    public int PointsWon { get; set; }
    public int PointsLost { get; set; }
    public int PointDifference { get; set; }
    public int Ranking { get; set; }
}