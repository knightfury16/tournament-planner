using Moq;
using TournamentPlanner.Application;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.GameTypeHandler;
using TournamentPlanner.Domain;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Enum;
using TournamentPlanner.Test.Fixtures;
using MatchType = TournamentPlanner.Domain.Entities.MatchType;

namespace TournamentPlanner.Test.Application.Services;

public class MatchTypeServiceTest
{
    private readonly Mock<ICreateMatchTypeFactory> _createMatchTypeFactoryMock;
    private readonly Mock<IRepository<MatchType>> _matchTypeRepositoryMock;
    private readonly Mock<IRepository<Draw>> _drawRepositoryMock;
    private readonly Mock<IRepository<Tournament>> _tournamentRepositoryMock;
    private readonly Mock<IGameFormatFactory> _gameFormatFactoryMock;
    private readonly MatchTypeService _sut;

    public MatchTypeServiceTest()
    {
        _createMatchTypeFactoryMock = new Mock<ICreateMatchTypeFactory>();
        _matchTypeRepositoryMock = new Mock<IRepository<MatchType>>();
        _drawRepositoryMock = new Mock<IRepository<Draw>>();
        _tournamentRepositoryMock = new Mock<IRepository<Tournament>>();
        _gameFormatFactoryMock = new Mock<IGameFormatFactory>();

        _sut = new MatchTypeService(
            _createMatchTypeFactoryMock.Object,
            _matchTypeRepositoryMock.Object,
            _drawRepositoryMock.Object,
            _gameFormatFactoryMock.Object,
            _tournamentRepositoryMock.Object
        );
    }

    [Fact]
    public async Task CreateMatchType_WithNoExistingDraws_CreatesFirstDraw()
    {
        // Arrange
        var tournament = TournamentFixtures.GetPopulatedGroupTournamentFresh();
        var expectedMatchTypes = new List<MatchType> { new Group { Name = "Group A" } };

        var matchTypeCreatorMock = new Mock<ICreateMatchType>();
        matchTypeCreatorMock
            .Setup(m => m.CreateMatchType(
                It.IsAny<Tournament>(),
                It.IsAny<List<Player>>(),
                It.IsAny<string>(),
                It.IsAny<List<int>>()))
            .ReturnsAsync(expectedMatchTypes);

        _createMatchTypeFactoryMock
            .Setup(f => f.GetMatchTypeCreator(TournamentType.GroupStage))
            .Returns(matchTypeCreatorMock.Object);

        // Act
        var result = await _sut.CreateMatchType(tournament);

        // Assert
        Assert.Equal(expectedMatchTypes, result);
        matchTypeCreatorMock.Verify(
            m => m.CreateMatchType(
                tournament,
                tournament.Participants,
                null,
                null),
            Times.Once);
    }

    [Fact]
    public async Task CreateMatchType_WithExistingDraws_CreatesKnockoutDraw()
    {
        // Arrange
        var tournament = TournamentFixtures.GetPopulatedGroupTournamentFresh();
        tournament.CurrentState = TournamentState.GroupState;
        var testGroup = new Group { Name = "Test Group A" };
        tournament.Draws = new List<Draw> { new Draw { Tournament = tournament, MatchType = testGroup } };

        var expectedMatchTypes = new List<MatchType> { testGroup };
        var matchTypeCreatorMock = new Mock<ICreateMatchType>();
        matchTypeCreatorMock
            .Setup(m => m.CreateMatchType(
                It.IsAny<Tournament>(),
                It.IsAny<List<Player>>(),
                It.IsAny<string>(),
                It.IsAny<List<int>>()))
            .ReturnsAsync(expectedMatchTypes);

        _createMatchTypeFactoryMock
            .Setup(f => f.GetMatchTypeCreator(TournamentType.Knockout))
            .Returns(matchTypeCreatorMock.Object);

        var gameFormatMock = new Mock<TableTennisGameFormat>();
        gameFormatMock
            .Setup(g => g.GetGroupStanding(It.IsAny<Tournament>(), It.IsAny<MatchType>(), false))
            .Returns(new List<PlayerStanding>());

        _gameFormatFactoryMock
            .Setup(f => f.GetGameFormat(It.IsAny<GameTypeSupported>()))
            .Returns(gameFormatMock.Object);

        // Act
        var result = await _sut.CreateMatchType(tournament);

        // Assert
        Assert.Equal(TournamentState.KnockoutState, tournament.CurrentState);
        Assert.Equal(expectedMatchTypes, result);
    }

    [Fact]
    public async Task UpdateMatchTypeCompletion_WithAllRoundsComplete_GroupMatchType_SetsMatchTypeAsComplete()
    {
        // Arrange
        var matchType = MatchTypeFixtures.GetGroup();
        var rounds = new List<Round>
        {
            new Round { MatchType = matchType, IsCompleted = true },
            new Round { MatchType = matchType, IsCompleted = true }
        };

        var matchTypeWithRounds = new Group { Id = 1, Name = "Test Group With Rounds", Rounds = rounds };

        _matchTypeRepositoryMock
            .Setup(r => r.GetByIdAsync(1, It.IsAny<string[]>()))
            .ReturnsAsync(matchTypeWithRounds);

        // Act
        await _sut.UpdateMatchTypeCompletion(matchType);

        // Assert
        Assert.True(matchType.IsCompleted);
        _matchTypeRepositoryMock.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateMatchTypeCompletion_WithAllRoundsComplete_KnockoutMatchType_SetsMatchTypeAsComplete()
    {
        // Arrange
        var matchType = MatchTypeFixtures.GetKnockOut();
        var matchTypeWithRounds = new KnockOut { Id = 1, Name = "Test Knockout With Rounds" };
        var rounds = new List<Round>
        {
            new Round { MatchType = matchType, IsCompleted = true },
            new Round { MatchType = matchType, IsCompleted = true }
        };
        matchTypeWithRounds.Rounds = rounds;

        _matchTypeRepositoryMock
            .Setup(r => r.GetByIdAsync(1, It.IsAny<string[]>()))
            .ReturnsAsync(matchTypeWithRounds);

        // Act
        await _sut.UpdateMatchTypeCompletion(matchType);

        // Assert
        Assert.True(matchType.IsCompleted);
        _matchTypeRepositoryMock.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateMatchTypeCompletion_WithIncompleteRounds_DoesNotSetMatchTypeAsComplete()
    {
        // Arrange
        var matchType = new Group { Id = 1, Name = "Test gorup A" };
        var rounds = new List<Round>
        {
            new Round { MatchType = matchType, IsCompleted = true },
            new Round { MatchType = matchType, IsCompleted = false }
        };

        var matchTypeWithRounds = new Group { Id = 1, Name = "Test Group with Rounds A", Rounds = rounds };

        _matchTypeRepositoryMock
            .Setup(r => r.GetByIdAsync(1, It.IsAny<string[]>()))
            .ReturnsAsync(matchTypeWithRounds);

        // Act
        await _sut.UpdateMatchTypeCompletion(matchType);

        // Assert
        Assert.False(matchType.IsCompleted);
        _matchTypeRepositoryMock.Verify(r => r.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task UpdateMatchTypeCompletion_WithNullMatchType_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.UpdateMatchTypeCompletion(null!));
    }
}
