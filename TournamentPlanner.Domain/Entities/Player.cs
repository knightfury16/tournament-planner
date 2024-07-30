namespace TournamentPlanner.Domain.Entities
{
    // public class Player<TScore> : User where TScore: IScore
    public class Player : User
    {
        public int Age { get; set; }
        public int Weight { get; set; }

        //How to store this value? should I just store TournamentId or the whole torunament?
        //!! keep this in mind, see the relationin database
        public List<Tournament> TournamentParticipated { get; set; } = new ();
        // public List<int> TournamentParticipatedId { get; set; } = new List<int>();

        // public List<GameType<TScore>> InterestedIn { get; set; } = new List<GameType<TScore>>();

        public int GamePlayed { get; set; }

        public int GameWon { get; set; }

        public double WinRation => GamePlayed > 0 ? (double)GameWon / GamePlayed : 0;

    }
}