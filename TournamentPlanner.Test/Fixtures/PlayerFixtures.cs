using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.Test.Fixtures;

public static class PlayerFixtures
{
    public static Player StandardPlayer => new Player
    {
        Id = 1,
        Name = "John Doe",
        Email = "john.doe@example.com",
        GamePlayed = 100,
        GameWon = 48
    };

    public static Player HighRatedPlayer => new Player
    {
        Id = 1,
        Name = "Alice Smith",
        Email = "alice.smith@example.com",
        GamePlayed = 100,
        GameWon = 88
    };

    public static Player JuniorPlayer => new Player
    {
        Id = 1,
        Name = "Mike Johnson",
        Email = "mike.johnson@example.com",
        GamePlayed = 10,
        GameWon = 3
    };
    public static Player UnderAgePlayer => new Player
    {
        Id = 1,
        Name = "Mike Johnson",
        Email = "mike.johnson@example.com",
        GamePlayed = 10,
        GameWon = 3,
        Age = 15
    };

    public static List<Player> GetSamplePlayers(int count = 5)
    {
        var players = new List<Player>();
        for (int i = 1; i <= count; i++)
        {
            players.Add(new Player
            {
                Id = i,
                Name = $"Player{i}",
                Email = $"player{i}@example.com",
            });
        }
        return players;
    }
}

