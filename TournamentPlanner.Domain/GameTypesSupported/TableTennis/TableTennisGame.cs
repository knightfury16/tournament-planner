using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.Domain.GameTypesSupported.TableTennis
{

    public class TableTennisGame : GameType<TableTennisScore>
    {
        public int SetsToWin { get; set; } = 3;
        public int PointsPerSet { get; set; } = 11;

        public override TableTennisScore CreateInitialScore()
        {
            return new TableTennisScore();
        }

        public override bool IsValidScore(TableTennisScore score)
        {
            if (score.Player1Sets + score.Player2Sets > SetsToWin * 2 - 1)
                return false;

            foreach (var setScore in score.SetScores)
            {
                if (setScore.Player1Points < PointsPerSet && setScore.Player2Points < PointsPerSet)
                    return false;
                if (Math.Abs(setScore.Player1Points - setScore.Player2Points) < 2)
                    return false;
            }

            return true;
        }

        public override Player DetermineWinner(Player player1, Player player2, TableTennisScore score)
        {
            return score.Player1Sets > score.Player2Sets ? player1 : player2;
        }

    }
}