using System.Linq.Expressions;
using Moq;
using TournamentPlanner.Application;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.Services;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Test.Fixtures;
using Match = TournamentPlanner.Domain.Entities.Match;
using MatchType = TournamentPlanner.Domain.Entities.MatchType;

namespace TournamentPlanner.Test.Application.Services;

public class RoundServiceTest
{
    private readonly Mock<IRepository<Round>> _roundRepositoryMock;
    private readonly Mock<IMatchService> _matchServiceMock;
    private readonly Mock<IMatchTypeService> _matchTypeServiceMock;
    private readonly Mock<IRepository<MatchType>> _matchTypeRepositoryMock;
    private readonly RoundService _sut;

    public RoundServiceTest()
    {
        _roundRepositoryMock = new Mock<IRepository<Round>>();
        _matchServiceMock = new Mock<IMatchService>();
        _matchTypeServiceMock = new Mock<IMatchTypeService>();
        _matchTypeRepositoryMock = new Mock<IRepository<MatchType>>();

        _sut = new RoundService(_roundRepositoryMock.Object, _matchServiceMock.Object, _matchTypeServiceMock.Object, _matchTypeRepositoryMock.Object);
    }

    [Fact]
    public async Task UpdateRoundCompletion_RoundIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        Round round = null!;
        // Act and Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.UpdateRoundCompletion(round));
    }

    [Fact]
    public async Task UpdateRoundCompletion_RoundIsCompleted_ReturnsImmediately()
    {
        // Arrange
        var round = new Round { MatchType = new KnockOut { Name = "Test Knockout" }, IsCompleted = true };

        // Act
        await _sut.UpdateRoundCompletion(round);

        // Assert
        _matchServiceMock.Verify(ms => ms.IsMatchComplete(It.IsAny<Match>()), Times.Never);
    }

    [Fact]
    public async Task UpdateRoundCompletion_MatchesAreNotCompleted_DoesNotUpdateRoundCompletion()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        var round = new Round { MatchType = new Group { Name = "Test Group" }, IsCompleted = false };
        var match = new Match { FirstPlayer = PlayerFixtures.StandardPlayer, SecondPlayer = PlayerFixtures.StandardPlayer, Round = round, Tournament = tournament };
        round.Matches.Add(match);

        _roundRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<string[]>())).ReturnsAsync(round);

        _matchServiceMock.Setup(ms => ms.IsMatchComplete(match)).ReturnsAsync(false);
        // Act
        await _sut.UpdateRoundCompletion(round);

        // Assert
        Assert.False(round.IsCompleted);
    }

    [Fact]
    public async Task UpdateRoundCompletion_MatchesAreCompleted_UpdatesRoundCompletion()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        var round = new Round { MatchType = new Group { Name = "Test Group" }, IsCompleted = false };
        var match = new Match { FirstPlayer = PlayerFixtures.StandardPlayer, SecondPlayer = PlayerFixtures.StandardPlayer, Round = round, Tournament = tournament };
        round.Matches.Add(match);

        _roundRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<string[]>())).ReturnsAsync(round);

        _matchServiceMock.Setup(ms => ms.IsMatchComplete(match)).ReturnsAsync(true);

        // Act
        await _sut.UpdateRoundCompletion(round);

        // Assert
        Assert.True(round.IsCompleted);
    }

    [Fact]
    public async Task IsAllRoundComplete_MatchTypeIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        MatchType matchType = null!;

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.IsAllRoundComplete(matchType));
    }

    [Fact]
    public async Task IsAllRoundComplete_NoRounds_ReturnsTrue()
    {
        // Arrange
        var matchType = new Group { Name = "Test Group", Rounds = new List<Round>() };

        _matchTypeRepositoryMock.Setup(mt => mt.ExplicitLoadCollectionAsync(matchType, mt => mt.Rounds)).Verifiable();
        _roundRepositoryMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Round, bool>>>())).ReturnsAsync(new List<Round>());

        // Act
        var result = await _sut.IsAllRoundComplete(matchType);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task IsAllRoundComplete_RoundsAreNotCompleted_ReturnsFalse()
    {
        // Arrange
        var matchType = new Group { Name = "Test Group", Rounds = new List<Round>() };
        var round = new Round { MatchType = matchType, IsCompleted = false };
        matchType.Rounds.Add(round);

        _matchTypeRepositoryMock.Setup(mt => mt.ExplicitLoadCollectionAsync(matchType, mt => mt.Rounds)).Verifiable();
        _roundRepositoryMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Round, bool>>>())).ReturnsAsync(new List<Round>(){round});

        // Act
        var result = await _sut.IsAllRoundComplete(matchType);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task IsAllRoundComplete_RoundsAreCompleted_ReturnsTrue()
    {
        // Arrange
        var matchType = new Group { Name = "Test Group", Rounds = new List<Round>() };
        var round = new Round { MatchType = matchType, IsCompleted = true };
        matchType.Rounds.Add(round);

        _matchTypeRepositoryMock.Setup(mt => mt.ExplicitLoadCollectionAsync(matchType, mt => mt.Rounds)).Verifiable();
        _roundRepositoryMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Round, bool>>>())).ReturnsAsync(new List<Round>(){round});

        // Act
        var result = await _sut.IsAllRoundComplete(matchType);

        // Assert
        Assert.True(result);
    }
}
