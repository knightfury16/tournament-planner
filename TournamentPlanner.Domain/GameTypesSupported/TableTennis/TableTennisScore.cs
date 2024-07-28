using TournamentPlanner.Domain.Interface;

namespace TournamentPlanner.Domain.GameTypesSupported.TableTennis
{
    public class TableTennisScore : IScore
    {
        public int Player1Sets { get; set; }
        public int Player2Sets { get; set; }
        public List<(int Player1Points, int Player2Points)> SetScores { get; set; } = new List<(int, int)>();
        public bool IsComplete => Player1Sets == 3 || Player2Sets == 3;
    }
}