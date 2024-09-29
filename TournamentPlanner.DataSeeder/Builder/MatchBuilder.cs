using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.DataSeeder
{
    public class MatchBuilder
    {
        private Player? _firstPlayer;
        private Player? _secondPlayer;
        private Tournament? _tournament;
        private Round? _round;
        private Match? _match;

        public MatchBuilder WithPlayers(Player player1, Player player2)
        {
            _firstPlayer = player1;
            _secondPlayer = player2;
            return this;
        }

        public MatchBuilder WithTournament(Tournament tournament)
        {
            _tournament = tournament;
            return this;
        }


        public MatchBuilder WithRound(Round round)
        {
            _round = round;
            return this;
        }

        public Match Build()
        {
            _match = new Match()
            {
                FirstPlayer = _firstPlayer!,
                SecondPlayer = _secondPlayer!,
                Tournament = _tournament!,
                Round = _round!
            };
            return _match;
        }

    }
}