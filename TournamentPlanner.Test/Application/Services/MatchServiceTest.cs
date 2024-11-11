using Moq;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Enum;
using TournamentPlanner.Application.Common.Interfaces;
using System.Linq.Expressions;
using TournamentPlanner.Test.Fixtures;
using TournamentPlanner.Application.Services;
using Match = TournamentPlanner.Domain.Entities.Match;
using TournamentPlanner.Application.Common;
using MatchType = TournamentPlanner.Domain.Entities.MatchType;
using TournamentPlanner.Domain;
using TournamentPlanner.Application.GameTypeHandler;
using TournamentPlanner.Domain.Exceptions;
using TournamentPlanner.Application.Helpers;

namespace TournamentPlanner.Application.UnitTests.Services;

public class MatchServiceTests
{
    private readonly Mock<IRepository<Draw>> _drawRepositoryMock;
    private readonly Mock<IRepository<Match>> _matchRepositoryMock;
    private readonly Mock<IRoundRobin> _roundRobinMock;
    private readonly Mock<IKnockout> _knockoutMock;
    private readonly Mock<IGameFormatFactory> _gameFormatFactoryMock;
    private readonly MatchService _matchService;

    public MatchServiceTests()
    {
        _drawRepositoryMock = new Mock<IRepository<Draw>>();
        _matchRepositoryMock = new Mock<IRepository<Match>>();
        _roundRobinMock = new Mock<IRoundRobin>();
        _knockoutMock = new Mock<IKnockout>();
        _gameFormatFactoryMock = new Mock<IGameFormatFactory>();

        _matchService = new MatchService(
            _drawRepositoryMock.Object,
            _matchRepositoryMock.Object,
            _roundRobinMock.Object,
            _gameFormatFactoryMock.Object,
            _knockoutMock.Object
        );
    }

    [Fact]
    public async Task CreateMaches_WithTournamentNull_ThrowsArgumentNullException()
    {
        //Act and Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _matchService.CreateMatches(null!, null!));
    }

    [Fact]
    public async Task CreateMatches_GroupState_CreatesGroupMatches()
    {
        // Arrange
        var tournament = TournamentFixtures.GetGroupTournament();
        tournament.CurrentState = TournamentState.GroupState;
        var testGroup1 = GetGroupMatchType(1);
        var testGroup2 = GetGroupMatchType(2);

        var draw1 = new Draw { Tournament = tournament, MatchType = testGroup1 };
        var draw2 = new Draw { Tournament = tournament, MatchType = testGroup2 };

        var matches1 = new List<Match> { CreateMatch(tournament, testGroup1), CreateMatch(tournament, testGroup1) };
        var matches2 = new List<Match> { CreateMatch(tournament, testGroup2), CreateMatch(tournament, testGroup2) };

        tournament.Draws.Add(draw1);
        tournament.Draws.Add(draw2);

        _roundRobinMock.Setup(x => x.CreateMatches(tournament, draw1.MatchType))
            .ReturnsAsync(matches1);
        _roundRobinMock.Setup(x => x.CreateMatches(tournament, draw2.MatchType))
            .ReturnsAsync(matches2);

        // Act
        var result = await _matchService.CreateMatches(tournament, null);

        // Assert
        Assert.Equal(4, result.Count());
        _roundRobinMock.Verify(x => x.CreateMatches(tournament, It.IsAny<Group>()), Times.Exactly(2));
    }

    [Fact]
    public async Task CreateMatches_KnockoutStateAfterGroup_CreatesFirstRoundMatchesAfterGroup()
    {
        // Arrange
        var tournament = TournamentFixtures.GetPopulatedGroupTournamentFresh(16);
        tournament.CurrentState = TournamentState.KnockoutState;
        tournament.TournamentType = TournamentType.GroupStage;
        var testGroup = GetGroupMatchType(1);
        var testKnockout = GetKnockoutMatchType();

        var knockoutDraw = new Draw
        {
            Id = 1,
            Tournament = tournament,
            MatchType = testKnockout
        };
        var groupDraw = new Draw
        {
            Id = 2,
            Tournament = tournament,
            MatchType = testGroup
        };

        tournament.Draws = new List<Draw> { knockoutDraw, groupDraw };

        var expectedMatches = new List<Match> { CreateMatch(tournament, testKnockout), CreateMatch(tournament, testKnockout) };
        var gameFormatMock = new Mock<TableTennisGameFormat>();
        var playerStandings = new List<PlayerStanding> { new PlayerStanding { Player = PlayerFixtures.StandardPlayer, Wins = 1, MatchPoints = 2 } };

        _drawRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<string[]>()))
            .ReturnsAsync((int id, string[] props) => tournament.Draws.First(d => d.Id == id));
        _gameFormatFactoryMock.Setup(x => x.GetGameFormat(tournament.GameType.Name))
            .Returns(gameFormatMock.Object);
        gameFormatMock.Setup(x => x.GetGroupStanding(tournament, testGroup, false))
            .Returns(playerStandings);
        _knockoutMock.Setup(x => x.CreateFirstRoundMatchesAfterGroup(
            tournament,
            knockoutDraw.MatchType,
            It.IsAny<Dictionary<string, List<PlayerStanding>>>()))
            .ReturnsAsync(expectedMatches);

        // Act
        var result = await _matchService.CreateMatches(tournament, null);

        // Assert
        Assert.Equal(expectedMatches.Count, result.Count());
        _knockoutMock.Verify(x => x.CreateFirstRoundMatchesAfterGroup(
            tournament,
            knockoutDraw.MatchType,
            It.IsAny<Dictionary<string, List<PlayerStanding>>>()),
            Times.Once);
    }

    [Fact]
    public async Task CreateMatches_CreatesFirstRoundMatchesAfterGroup_WithNoGroupStanding_ThrowException()
    {
        // Arrange
        var tournament = TournamentFixtures.GetPopulatedGroupTournamentFresh(16);
        tournament.CurrentState = TournamentState.KnockoutState;
        tournament.TournamentType = TournamentType.GroupStage;
        var testGroup = GetGroupMatchType(1);
        var testKnockout = GetKnockoutMatchType();

        var knockoutDraw = new Draw
        {
            Id = 1,
            Tournament = tournament,
            MatchType = testKnockout
        };
        var groupDraw = new Draw
        {
            Id = 2,
            Tournament = tournament,
            MatchType = testGroup
        };

        tournament.Draws = new List<Draw> { knockoutDraw, groupDraw };

        var expectedMatches = new List<Match> { CreateMatch(tournament, testKnockout), CreateMatch(tournament, testKnockout) };
        var gameFormatMock = new Mock<TableTennisGameFormat>();
        var playerStandings = new List<PlayerStanding>();

        _drawRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<string[]>()))
            .ReturnsAsync((int id, string[] props) => tournament.Draws.First(d => d.Id == id));
        _gameFormatFactoryMock.Setup(x => x.GetGameFormat(tournament.GameType.Name))
            .Returns(gameFormatMock.Object);
        gameFormatMock.Setup(x => x.GetGroupStanding(tournament, testGroup, false))
            .Returns(playerStandings);
        _knockoutMock.Setup(x => x.CreateFirstRoundMatchesAfterGroup(
            tournament,
            knockoutDraw.MatchType,
            It.IsAny<Dictionary<string, List<PlayerStanding>>>()))
            .ReturnsAsync(expectedMatches);

        //Act and  Assert
        await Assert.ThrowsAsync<Exception>(() => _matchService.CreateMatches(tournament, null));
    }



    [Fact]
    public async Task CreateMatches_KnockoutStateAfterGroup_WithNoGroupStanding_ThrowsException()
    {
        // Arrange
        var tournament = TournamentFixtures.GetKnockoutTournament();
        tournament.CurrentState = TournamentState.KnockoutState;
        tournament.TournamentType = TournamentType.GroupStage;
        var testKnockout = GetKnockoutMatchType();

        var knockoutDraw = new Draw
        {
            Id = 1,
            Tournament = tournament,
            MatchType = testKnockout
        };
        tournament.Draws = new List<Draw> { knockoutDraw };

        _drawRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<string[]>()))
            .ReturnsAsync((int id, string[] props) => tournament.Draws.First(d => d.Id == id));

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() => _matchService.CreateMatches(tournament, null));
    }

    [Fact]
    public async Task CreateMatches_KnockoutStateAfterGroup_WithNoKnockoutDraws_ThrowsException()
    {
        // Arrange
        var tournament = TournamentFixtures.GetKnockoutTournament();
        tournament.CurrentState = TournamentState.KnockoutState;
        tournament.TournamentType = TournamentType.GroupStage;

        var groupDraw = new Draw
        {
            Id = 2,
            Tournament = tournament,
            MatchType = new Group { Name = "Group A" }
        };
        tournament.Draws = new List<Draw> { groupDraw };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _matchService.CreateMatches(tournament, null));
    }

    [Fact]
    public async Task CreateMatches_KnockoutStateAfterGroup_WithNullGameType_ThrowsException()
    {
        // Arrange
        var tournament = TournamentFixtures.GetPopulatedGroupTournamentFresh(16);
        tournament.CurrentState = TournamentState.KnockoutState;
        tournament.TournamentType = TournamentType.GroupStage;
        tournament.GameType = null!; // Simulate invalid game type
        var knockoutTest = GetKnockoutMatchType();
        var groupTest = GetGroupMatchType(1);

        var knockoutDraw = new Draw
        {
            Id = 1,
            Tournament = tournament,
            MatchType = knockoutTest
        };
        var groupDraw = new Draw
        {
            Id = 2,
            Tournament = tournament,
            MatchType = groupTest
        };
        tournament.Draws = new List<Draw> { knockoutDraw, groupDraw };

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() => _matchService.CreateMatches(tournament, null));
    }




    [Fact]
    public async Task CreateMatches_KnockoutState_FirstRound_CreateFirstRoundMatches()
    {
        // Arrange
        var tournament = TournamentFixtures.GetKnockoutTournament();
        tournament.CurrentState = TournamentState.KnockoutState;
        var testKnockout = GetKnockoutMatchType();
        var knockoutDraw = new Draw
        {
            Tournament = tournament,
            Id = 1,
            MatchType = testKnockout
        };
        tournament.Draws = new List<Draw> { knockoutDraw };

        var expectedMatches = new List<Match> { CreateMatch(tournament, testKnockout), CreateMatch(tournament, testKnockout) };

        _drawRepositoryMock.Setup(x => x.GetByIdAsync(1, It.IsAny<string[]>()))
            .ReturnsAsync(knockoutDraw);
        _knockoutMock.Setup(x => x.CreateFirstRoundMatches(tournament, knockoutDraw.MatchType))
            .ReturnsAsync(expectedMatches);

        // Act
        var result = await _matchService.CreateMatches(tournament, null);

        // Assert
        Assert.Equal(expectedMatches.Count, result.Count());
        _knockoutMock.Verify(x => x.CreateFirstRoundMatches(tournament, knockoutDraw.MatchType), Times.Once);
    }

    [Fact]
    public async Task CreateMatches_KnockoutState_SubsequentRound_CreatesSubsequentMatches()
    {
        // Arrange
        var tournament = TournamentFixtures.GetKnockoutTournament();
        tournament.CurrentState = TournamentState.KnockoutState;
        var testKnockout = GetKnockoutMatchType();
        var testRound1 = new Round { MatchType = testKnockout, RoundName = "first", RoundNumber = 1 };
        var testRound2 = new Round { MatchType = testKnockout, RoundName = "second", RoundNumber = 2 };

        testKnockout.Rounds.Add(testRound1);
        testKnockout.Rounds.Add(testRound2);

        var knockoutDraw = new Draw
        {
            Id = 1,
            Tournament = tournament,
            MatchType = testKnockout
        };
        tournament.Draws = new List<Draw> { knockoutDraw };

        var expectedMatches = new List<Match> { CreateMatch(tournament, testKnockout) };

        _drawRepositoryMock.Setup(x => x.GetByIdAsync(1, It.IsAny<string[]>()))
            .ReturnsAsync(knockoutDraw);
        _knockoutMock.Setup(x => x.CreateSubsequentMatches(tournament, knockoutDraw.MatchType))
            .ReturnsAsync(expectedMatches);

        // Act
        var result = await _matchService.CreateMatches(tournament, null);

        // Assert
        Assert.Equal(expectedMatches.Count, result.Count());
        _knockoutMock.Verify(x => x.CreateSubsequentMatches(tournament, knockoutDraw.MatchType), Times.Once);
    }

    [Fact]
    public async Task IsMatchComplete_WithBothPlayersBye_ThrowsException()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        var testGroup = GetGroupMatchType(1);
        var match = CreateMatch(tournament, testGroup);
        match.FirstPlayer = new Player { Name = Utility.ByePlayerName, Email = Utility.ByePlayerEmail };
        match.SecondPlayer = new Player { Name = Utility.ByePlayerName, Email = Utility.ByePlayerEmail };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _matchService.IsMatchComplete(match));
    }

    [Fact]
    public async Task IsMatchComplete_WithOnePlayerBye_ReturnsTrue()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        var testGroup = GetGroupMatchType(1);
        var match = CreateMatch(tournament, testGroup);

        match.FirstPlayer = PlayerFixtures.StandardPlayer;
        match.SecondPlayer = new Player { Name = Utility.ByePlayerName, Email = Utility.ByePlayerEmail };

        // Act
        var result = await _matchService.IsMatchComplete(match);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task IsMatchComplete_WithScoreJson_ReturnsTrue()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        var testGroup = GetGroupMatchType(1);
        var match = CreateMatch(tournament, testGroup);
        match.FirstPlayer = PlayerFixtures.StandardPlayer;
        match.SecondPlayer = PlayerFixtures.HighRatedPlayer;
        match.ScoreJson = new { Score = "21-19" };

        // Act
        var result = await _matchService.IsMatchComplete(match);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task IsMatchComplete_WithNoScoreAndNoByePlayers_ReturnsFalse()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        var testGroup = GetGroupMatchType(1);
        var match = CreateMatch(tournament, testGroup);
        match.FirstPlayer = PlayerFixtures.StandardPlayer;
        match.SecondPlayer = PlayerFixtures.HighRatedPlayer;

        // Act
        var result = await _matchService.IsMatchComplete(match);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task IsMatchComplete_WithNullPlayers_LoadsPlayersAndChecksCompletion()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        var testGroup = GetGroupMatchType(1);
        var match = CreateMatch(tournament, testGroup);
        match.FirstPlayer = null!;
        match.SecondPlayer = null!;

        _matchRepositoryMock.Setup(x => x.ExplicitLoadReferenceAsync(match, It.IsAny<Expression<Func<Match, Player?>>>()))
            .Callback<Match, Expression<Func<Match, Player?>>>((m, exp) =>
            {
                if (exp.Body.ToString().Contains("FirstPlayer"))
                    m.FirstPlayer = PlayerFixtures.StandardPlayer;
                else
                    m.SecondPlayer = PlayerFixtures.HighRatedPlayer;
            });

        // Act
        var result = await _matchService.IsMatchComplete(match);

        // Assert
        Assert.False(result);
        _matchRepositoryMock.Verify(x => x.ExplicitLoadReferenceAsync(match, It.IsAny<Expression<Func<Match, Player?>>>()), Times.Exactly(2));
    }

    private Match CreateMatch(Tournament tournament, MatchType matchType)
    {
        return new Match
        {
            FirstPlayer = PlayerFixtures.StandardPlayer,
            SecondPlayer = PlayerFixtures.HighRatedPlayer,
            Tournament = tournament,
            Round = new Round { MatchType = matchType }
        };
    }
    private MatchType GetGroupMatchType(int groupNumber)
    {
        return new Group
        {
            Name = $"Test Group {groupNumber}",
        };
    }
    private KnockOut GetKnockoutMatchType()
    {
        return new KnockOut { Name = "Test Knockout", Rounds = new List<Round>() };
    }
}