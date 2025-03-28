using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.Test.Fixtures;

public static class PlayerFixtures
{
    public static Player StandardPlayer =>
        new Player
        {
            Id = 1,
            Name = "John Doe",
            Email = "john.doe@example.com",
        };

    public static Player HighRatedPlayer =>
        new Player
        {
            Id = 1,
            Name = "Alice Smith",
            Email = "alice.smith@example.com",
        };

    public static Player JuniorPlayer =>
        new Player
        {
            Id = 1,
            Name = "Mike Johnson",
            Email = "mike.johnson@example.com",
        };
    public static Player UnderAgePlayer =>
        new Player
        {
            Id = 1,
            Name = "Mike Johnson",
            Email = "mike.johnson@example.com",
            Age = 15,
        };

    public static List<Player> GetSamplePlayers(int count = 5)
    {
        var players = new List<Player>();
        for (int i = 1; i <= count; i++)
        {
            players.Add(
                new Player
                {
                    Id = i,
                    Name = $"Player{i}",
                    Email = $"player{i}@example.com",
                }
            );
        }
        return players;
    }

    public static PlayerDto ToPlayerDto(this Player player)
    {
        return new PlayerDto
        {
            Id = player.Id,
            Name = player.Name,
            Age = player.Age,
        };
    }

    public static GameType GetTableTennisGameType()
    {
        return new GameType { Name = Domain.Enum.GameTypeSupported.TableTennis };
    }

    public static GameType GetEightBallGameType()
    {
        return new GameType { Name = Domain.Enum.GameTypeSupported.EightBallPool };
    }
}
