using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Interface;

namespace TournamentPlanner.Application.GameTypeScore;
//configuring SetsToWin and PointsPerSet so that each mach can be configured separately
public class TableTennisScore : IScore
{
    public int Player1Sets { get; set; }
    public int Player2Sets { get; set; }
    public int SetsToWin { get; set; } = 3; //deafult 3
    public int PointsPerSet { get; set; } = 11; // default 11
    public List<SetScore> SetScores { get; set; } = new();
    public bool IsComplete => Player1Sets == SetsToWin || Player2Sets == SetsToWin;
}
