using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.Test.Fixtures;

public static class RoundFixtures
{
    public static Round GetRoundOfKnockout(string roundName = "Test", int roundNumber = 1)
    {
        return new Round { RoundName = roundName, RoundNumber = roundNumber, MatchType = MatchTypeFixtures.GetKnockOut() };
    }
    public static Round GetRoundOfGroup(string roundName = "Test", int roundNumber = 1)
    {
        return new Round { RoundName = roundName, RoundNumber = roundNumber, MatchType = MatchTypeFixtures.GetGroup() };
    }
}
