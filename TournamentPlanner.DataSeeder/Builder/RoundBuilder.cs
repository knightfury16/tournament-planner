
using TournamentPlanner.Domain.Entities;
using MatchType = TournamentPlanner.Domain.Entities.MatchType;

namespace TournamentPlanner.DataSeeder
{
    public class RoundBuilder
    {

        private Round? _round;
        private MatchType? _matchType;
        private int _roundNumber = 1;
        private string _roundName = "Test Round";
        private List<Match> _matches = new List<Match>();
        private DateTime _roundDate = DateTime.UtcNow.AddDays(10);
        public RoundBuilder WithMatchType(MatchType matchType)
        {
            _matchType = matchType;
            return this;
        }
        public RoundBuilder WithRoundNumber(int roundNumber)
        {
            _roundNumber = roundNumber;
            return this;
        }
        public RoundBuilder WithRoundName(string roundName)
        {
            _roundName = roundName;
            return this;
        }

        public RoundBuilder WithMatches(List<Match> matches)
        {
            _matches = matches;
            return this;
        }
        public RoundBuilder WithStartTime(DateTime startTime)
        {
            _roundDate = startTime;
            return this;
        }
        public Round Build()
        {
            _round = new Round
            {
                MatchType = _matchType!,
                RoundNumber = _roundNumber,
                RoundName = _roundName,
                Matches = _matches,
                StartTime = _roundDate,
            };
            return _round;
        }
    }
}