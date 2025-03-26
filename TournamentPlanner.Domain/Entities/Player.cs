namespace TournamentPlanner.Domain.Entities;

public class Player : User
{
    public int Age { get; set; }
    public int Weight { get; set; }
    public List<GameStatistic>? GameStatistics { get; set; }
    public List<Tournament>? Tournaments { get; set; }
    public List<MatchType>? MatchTypes { get; set; }
    public List<SeededPlayer>? SeededEntries { get; set; }
}
