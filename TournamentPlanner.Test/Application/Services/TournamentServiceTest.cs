using Moq;
using TournamentPlanner.Application;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Enum;
using TournamentPlanner.Test.Fixtures;
using Match = TournamentPlanner.Domain.Entities.Match;
using MatchType = TournamentPlanner.Domain.Entities.MatchType;

namespace TournamentPlanner.Test.Application.Services;

public class TournamentServiceTest
{
    private readonly Mock<IDrawService> _drawServiceMock;
    private readonly Mock<IMatchTypeService> _matchTypeServiceMock;
    private readonly Mock<IRepository<Tournament>> _tournamentRepositoryMock;
    private readonly Mock<IRoundService> _roundServiceMock;
    private readonly TournamentService _sut;

    public TournamentServiceTest()
    {
        _drawServiceMock = new Mock<IDrawService>();
        _matchTypeServiceMock = new Mock<IMatchTypeService>();
        _tournamentRepositoryMock = new Mock<IRepository<Tournament>>();
        _roundServiceMock = new Mock<IRoundService>();
        _sut = new TournamentService(_drawServiceMock.Object, _matchTypeServiceMock.Object, _tournamentRepositoryMock.Object, _roundServiceMock.Object);
    }

    [Fact]
    public async Task CanIMakeDraw_WithNoDraws_ReturnsTrue()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        tournament.Draws = new List<Draw>();

        // Act
        var result = await _sut.CanIMakeDraw(tournament);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CanIMakeDraw_InGroupStateWithCompleteDraws_ReturnsTrue()
    {
        // Arrange
        var tournament = TournamentFixtures.GetGroupTournament();
        tournament.Draws = new List<Draw>();
        tournament.CurrentState = TournamentState.GroupState;

        _drawServiceMock.Setup(x => x.IsDrawsComplete(tournament)).ReturnsAsync(true);

        // Act
        var result = await _sut.CanIMakeDraw(tournament);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CanIMakeDraw_InKnockoutState_ReturnsFalse()
    {
        // Arrange
        var tournament = TournamentFixtures.GetKnockoutTournament();
        tournament.Draws = new List<Draw> {
                new Draw { Tournament = tournament, MatchType = new Group { Name = "Group A" } }
            };
        tournament.CurrentState = TournamentState.KnockoutState;


        // Act
        var result = await _sut.CanIMakeDraw(tournament);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task CanISchedule_WithNoDraws_ReturnsFalse()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        tournament.Draws = new List<Draw>();

        // Act
        var result = await _sut.CanISchedule(tournament);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task CanISchedule_WithDrawsButNoMatches_ReturnsTrue()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        tournament.Draws = new List<Draw> { new Draw { Tournament = tournament, MatchType = new Group { Name = "Group A" } } };
        tournament.Matches = new List<Match>();

        // Act
        var result = await _sut.CanISchedule(tournament);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task MakeDraws_WithValidSeeders_ReturnsDraws()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        tournament.Participants.AddRange(PlayerFixtures.GetSamplePlayers(2));

        var seeders = new List<int> { 1 };

        var matchType = new Group { Name = "Group A" };
        _matchTypeServiceMock.Setup(x => x.CreateMatchType(tournament, It.IsAny<string>(), seeders))
            .ReturnsAsync(new List<MatchType> { matchType });

        // Act
        var result = await _sut.MakeDraws(tournament, null, seeders);

        // Assert
        Assert.Single(result);
        Assert.Equal(matchType, result.First().MatchType);
    }

    [Fact]
    public async Task MakeDraws_WithInvalidSeeders_ThrowsException()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        tournament.Participants.AddRange(PlayerFixtures.GetSamplePlayers(2));

        var seeders = new List<int> { 3 }; // Invalid seeder

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() =>
            _sut.MakeDraws(tournament, null, seeders));
    }
}
