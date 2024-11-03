namespace TournamentPlanner.Test;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TournamentPlanner.Application.Common;
using TournamentPlanner.Domain.Exceptions;
using TournamentPlanner.Test.Fixtures;
using Xunit;


public class CreateGroupMatchTypeTest
{
    private readonly CreateGroupMatchType _sut;

    public CreateGroupMatchTypeTest()
    {
        _sut = new CreateGroupMatchType();
    }

    [Fact]
    public async Task CreateMatchType_WithNoSeededPlayers_CreatesGroupsAndDistributesPlayers()
    {
        // Arrange
        var tournament = TournamentFixtures.GetPopulatedGroupTournamentFresh(16);
        tournament.KnockOutStartNumber = 8;
        tournament.WinnerPerGroup = 2;

        // Act
        var result = await _sut.CreateMatchType(tournament, tournament.Participants, "Group", null);

        // Assert
        Assert.NotNull(result);
        var groups = result!.ToList();
        Assert.Equal(4, groups.Count); // 8 players needed / 2 winners per group = 4 groups
        Assert.All(groups, g => Assert.Equal(4, g.Players.Count())); // 16 players / 4 groups = 4 players per group
        Assert.All(groups, g => Assert.Empty(g.SeededPlayers));
        Assert.Collection(groups,
            g => Assert.Equal("Group-A", g.Name),
            g => Assert.Equal("Group-B", g.Name),
            g => Assert.Equal("Group-C", g.Name),
            g => Assert.Equal("Group-D", g.Name)
        );
    }

    [Fact]
    public async Task CreateMatchType_WithSeededPlayers_DistributesSeededPlayersFirst()
    {
        // Arrange
        var tournament = TournamentFixtures.GetPopulatedGroupTournamentFresh(16);
        tournament.KnockOutStartNumber = 8;
        tournament.WinnerPerGroup = 2;
        var seededPlayerIds = new List<int> { 1, 2, 3, 4 };

        // Act
        var result = await _sut.CreateMatchType(tournament, tournament.Participants, "Group", seededPlayerIds);

        // Assert
        Assert.NotNull(result);
        var groups = result!.ToList();
        Assert.Equal(4, groups.Count());
        Assert.All(groups, g => Assert.Single(g.SeededPlayers));
        Assert.All(groups, g => Assert.Contains(g.Players, p => seededPlayerIds.Contains(p.Id)));
    }

    [Fact]
    public async Task CreateMatchType_WithCustomPrefix_UsesProvidedPrefix()
    {
        // Arrange
        var tournament = TournamentFixtures.GetPopulatedGroupTournamentFresh(8);
        tournament.KnockOutStartNumber = 4;
        tournament.WinnerPerGroup = 2;
        var customPrefix = "Pool";

        // Act
        var result = await _sut.CreateMatchType(tournament, tournament.Participants, customPrefix, null);

        // Assert
        Assert.NotNull(result);
        var groups = result!.ToList();
        Assert.All(groups, g => Assert.StartsWith(customPrefix, g.Name));
    }

    [Fact]
    public async Task CreateMatchType_WithDefaultPrefix_UsesGroupPrefix()
    {
        // Arrange
        var tournament = TournamentFixtures.GetPopulatedGroupTournamentFresh(8);
        tournament.KnockOutStartNumber = 4;
        tournament.WinnerPerGroup = 2;

        // Act
        var result = await _sut.CreateMatchType(tournament, tournament.Participants, null, null);

        // Assert
        Assert.NotNull(result);
        var groups = result!.ToList();
        Assert.All(groups, g => Assert.StartsWith("Group", g.Name));
    }

    [Fact]
    public async Task CreateMatchType_WithOddNumberOfPlayers_DistributesPlayersEvenly()
    {
        // Arrange
        var tournament = TournamentFixtures.GetPopulatedGroupTournamentFresh(15);
        tournament.KnockOutStartNumber = 8;
        tournament.WinnerPerGroup = 2;

        // Act
        var result = await _sut.CreateMatchType(tournament, tournament.Participants, null, null);

        // Assert
        Assert.NotNull(result);
        var groups = result!.ToList();
        Assert.Equal(4, groups.Count());
        Assert.Equal(3, groups.Count(g => g.Players.Count() == 4));
        Assert.Single(groups, g => g.Players.Count() == 3);
    }

    [Fact]
    public async Task CreateMatchType_WithMoreSeededPlayersThanGroups_DistributesSeededPlayersEvenly()
    {
        // Arrange
        var tournament = TournamentFixtures.GetPopulatedGroupTournamentFresh(16);
        tournament.KnockOutStartNumber = 8;
        tournament.WinnerPerGroup = 2;
        var seededPlayerIds = new List<int> { 1, 2, 3, 4, 5, 6 };

        // Act
        var result = await _sut.CreateMatchType(tournament, tournament.Participants, null, seededPlayerIds);

        // Assert
        Assert.NotNull(result);
        var groups = result!.ToList();
        Assert.Equal(4, groups.Count());
        Assert.Equal(2, groups.Count(g => g.SeededPlayers.Count() == 2));
        Assert.Equal(2, groups.Count(g => g.SeededPlayers.Count() == 1));
    }

    [Fact]
    public async Task CreateMatchType_WithFewerPlayersThanKnockoutStartNumber_ThrowsException()
    {
        // Arrange
        var tournament = TournamentFixtures.GetPopulatedGroupTournamentFresh(6);
        tournament.KnockOutStartNumber = 8;
        tournament.WinnerPerGroup = 2;

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => 
            _sut.CreateMatchType(tournament, tournament.Participants, null, null));
    }
}
