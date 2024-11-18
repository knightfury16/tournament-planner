using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.Test.Fixtures;

public static class MatchFixtures
{
    public static Match GetSinglemMatchOfGroup()
    {
        var tournament = TournamentFixtures.GetTournament();
        var firstPlayer = PlayerFixtures.StandardPlayer;
        var secondPlayer = PlayerFixtures.StandardPlayer;
        var round = RoundFixtures.GetRoundOfGroup();
        return new Match
        {
            Tournament = tournament,
            FirstPlayer = firstPlayer,
            SecondPlayer = secondPlayer,
            Round = round
        };

    }

    public static Match GetSingleMatchOfKnockout()
    {
        var tournament = TournamentFixtures.GetTournament();
        var firstPlayer = PlayerFixtures.StandardPlayer;
        var secondPlayer = PlayerFixtures.StandardPlayer;
        var round = RoundFixtures.GetRoundOfKnockout();
        return new Match
        {
            Tournament = tournament,
            FirstPlayer = firstPlayer,
            SecondPlayer = secondPlayer,
            Round = round
        };
    }

    public static List<Match> GetMatches(int matchCount = 5)
    {
        var matches = new List<Match>();
        for (int i = 0; i < matchCount; i++)
        {
            matches.Add(GetSinglemMatchOfGroup());
        }
        return matches;
    }
}
