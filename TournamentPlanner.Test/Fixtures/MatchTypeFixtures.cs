using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.Test.Fixtures;

public static class MatchTypeFixtures
{
    
    public static KnockOut GetKnockOut(string name = "Test Knockout")
    {
        return new KnockOut
        {
            Name = name,
        };
    }
    public static Group GetGroup(string name = "Test Group")
    {
        return new Group
        {
            Name = name,
        };
    }
}
