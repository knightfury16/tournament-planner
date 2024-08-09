namespace TournamentPlanner.Domain.Entities;

public class Player : User
{
    public int Age { get; set; }
    public int Weight { get; set; }
    public int GamePlayed { get; set; }
    public int GameWon { get; set; }
    public double WinRatio => GamePlayed > 0 ? (double)GameWon / GamePlayed : 0;
    public List<Tournament>? Tournament { get; set; }
    public List<MatchType>? MatchTypes { get; set; }
}
