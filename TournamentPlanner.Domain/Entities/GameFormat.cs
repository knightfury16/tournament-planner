using TournamentPlanner.Domain.Interface;

namespace TournamentPlanner.Domain.Entities
{
    public abstract class GameFormat<TScore> where TScore : Score
    {
        public required GameType GameType { get; set; }
        public abstract TScore CreateInitialScore();
        public abstract bool IsValidScore(TScore score);
        public abstract Player DetermineWinner(Player player1, Player player2, TScore score);
    }
}