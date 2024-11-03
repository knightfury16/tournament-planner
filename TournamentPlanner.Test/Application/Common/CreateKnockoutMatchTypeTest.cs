using TournamentPlanner.Application.Common;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Test.Fixtures;

namespace TournamentPlanner.Test;

public class CreateKnockoutMatchTypeTest
{
    private readonly CreateKnockoutMatchType _sut;

    public CreateKnockoutMatchTypeTest()
    {
        _sut = new CreateKnockoutMatchType();
    }

    [Fact]
    public async Task CreateMatchType_WithNoSeededPlayers_CreatesKnockout()
    {
        // Arrange
        var tournament = TournamentFixtures.GetKnockoutTournament();
        var players = PlayerFixtures.GetSamplePlayers(8);

        // Act
        var result = await _sut.CreateMatchType(tournament, players, "Knockout", null);

        // Assert
        Assert.NotNull(result);
        var matchTypes = result!.ToList();
        Assert.Single(matchTypes);
        Assert.IsType<KnockOut>(matchTypes[0]);
        Assert.Equal("Knockout", matchTypes[0].Name);
        Assert.Equal(8, matchTypes[0].Players.Count);
    }

    [Fact]
    public async Task CreateMatchType_WithSeededPlayers_CreatesSeededKnockout()
    {
        // Arrange
        var tournament = TournamentFixtures.GetKnockoutTournament();
        var players = PlayerFixtures.GetSamplePlayers(8);
        var seederPlayerIds = new List<int> { 1, 2, 3, 4 };

        // Act
        var result = await _sut.CreateMatchType(tournament, players, "Knockout", seederPlayerIds);

        // Assert
        Assert.NotNull(result);
        var matchTypes = result!.ToList();
        Assert.Single(matchTypes);
        Assert.IsType<KnockOut>(matchTypes[0]);
        Assert.Equal("Knockout", matchTypes[0].Name);
        Assert.Equal(8, matchTypes[0].Players.Count);
        Assert.Equal(4, matchTypes[0].SeededPlayers.Count);
    }

    [Fact]
    public async Task CreateMatchType_WithCustomPrefix_UsesProvidedPrefix()
    {
        // Arrange
        var tournament = TournamentFixtures.GetKnockoutTournament();
        var players = PlayerFixtures.GetSamplePlayers(8);
        var customPrefix = "CustomKnockout";

        // Act
        var result = await _sut.CreateMatchType(tournament, players, customPrefix, null);

        // Assert
        Assert.NotNull(result);
        var matchTypes = result!.ToList();
        Assert.Single(matchTypes);
        Assert.IsType<KnockOut>(matchTypes[0]);
        Assert.Equal(customPrefix, matchTypes[0].Name);
        Assert.Equal(8, matchTypes[0].Players.Count);
    }

    [Fact]
    public async Task CreateMatchType_WithDefaultPrefix_UsesKnockoutPrefix()
    {
        // Arrange
        var tournament = TournamentFixtures.GetKnockoutTournament();
        var players = PlayerFixtures.GetSamplePlayers(8);

        // Act
        var result = await _sut.CreateMatchType(tournament, players, null, null);

        // Assert
        Assert.NotNull(result);
        var matchTypes = result!.ToList();
        Assert.Single(matchTypes);
        Assert.IsType<KnockOut>(matchTypes[0]);
        Assert.Equal("Knockout", matchTypes[0].Name);
        Assert.Equal(8, matchTypes[0].Players.Count);
    }



}
