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

public class MatchServiceTests
{
    private readonly Mock<IRepository<Draw>> _drawRepositoryMock;
    private readonly Mock<IRepository<Match>> _matchRepositoryMock;
    private readonly Mock<IRepository<Tournament>> _tournamentRepositoryMock;
    private readonly Mock<IRoundRobin> _roundRobinMock;
    private readonly Mock<IKnockout> _knockoutMock;
    private readonly Mock<IGameFormatFactory> _gameFormatFactoryMock;
    private readonly MatchService _matchService;

    public MatchServiceTests()
    {
        _drawRepositoryMock = new Mock<IRepository<Draw>>();
        _matchRepositoryMock = new Mock<IRepository<Match>>();
        _tournamentRepositoryMock = new Mock<IRepository<Tournament>>();
        _roundRobinMock = new Mock<IRoundRobin>();
        _knockoutMock = new Mock<IKnockout>();
        _gameFormatFactoryMock = new Mock<IGameFormatFactory>();

        _matchService = new MatchService(
            _drawRepositoryMock.Object,
            _matchRepositoryMock.Object,
            _roundRobinMock.Object,
            _gameFormatFactoryMock.Object,
            _knockoutMock.Object,
            _tournamentRepositoryMock.Object
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


    // [Fact]
    // public async Task CreateMatches_KnockoutState_FirstRound_CreateFirstRoundMatches()
    // {
    //     // Arrange
    //     var tournament = TournamentFixtures.GetKnockoutTournament();
    //     tournament.CurrentState = TournamentState.KnockoutState;
    //     var knockoutDraw = new Draw
    //     {
    //         Id = 1,
    //         MatchType = new KnockOut { Rounds = new List<Round>() }
    //     };
    //     tournament.Draws = new List<Draw> { knockoutDraw };

    //     var expectedMatches = new List<Match> { CreateMatch(tournament), CreateMatch(tournament) };

    //     _drawRepositoryMock.Setup(x => x.GetByIdAsync(1, It.IsAny<string[]>()))
    //         .ReturnsAsync(knockoutDraw);
    //     _knockoutMock.Setup(x => x.CreateFirstRoundMatches(tournament, knockoutDraw.MatchType))
    //         .ReturnsAsync(expectedMatches);

    //     // Act
    //     var result = await _matchService.CreateMatches(tournament, null);

    //     // Assert
    //     Assert.Equal(expectedMatches.Count, result.Count());
    //     _knockoutMock.Verify(x => x.CreateFirstRoundMatches(tournament, knockoutDraw.MatchType), Times.Once);
    // }

    // [Fact]
    // public async Task CreateMatches_KnockoutState_SubsequentRound_CreatesSubsequentMatches()
    // {
    //     // Arrange
    //     var tournament = TournamentFixtures.GetKnockoutTournament();
    //     tournament.CurrentState = TournamentState.KnockoutState;
    //     var knockoutDraw = new Draw
    //     {
    //         Id = 1,
    //         MatchType = new KnockOut
    //         {
    //             Rounds = new List<Round> { new Round(), new Round() }
    //         }
    //     };
    //     tournament.Draws = new List<Draw> { knockoutDraw };

    //     var expectedMatches = new List<Match> { CreateMatch(tournament) };

    //     _drawRepositoryMock.Setup(x => x.GetByIdAsync(1, It.IsAny<string[]>()))
    //         .ReturnsAsync(knockoutDraw);
    //     _knockoutMock.Setup(x => x.CreateSubsequentMatches(tournament, knockoutDraw.MatchType))
    //         .ReturnsAsync(expectedMatches);

    //     // Act
    //     var result = await _matchService.CreateMatches(tournament, null);

    //     // Assert
    //     Assert.Equal(expectedMatches.Count, result.Count());
    //     _knockoutMock.Verify(x => x.CreateSubsequentMatches(tournament, knockoutDraw.MatchType), Times.Once);
    // }

    // [Fact]
    // public async Task CreateMatches_KnockoutStateAfterGroup_CreatesFirstRoundMatchesAfterGroup()
    // {
    //     // Arrange
    //     var tournament = TournamentFixtures.GetPopulatedGroupTournamentFresh(16);
    //     tournament.CurrentState = TournamentState.KnockoutState;
    //     tournament.TournamentType = TournamentType.GroupAndKnockout;

    //     var knockoutDraw = new Draw
    //     {
    //         Id = 1,
    //         MatchType = new KnockOut { Rounds = new List<Round>() }
    //     };
    //     var groupDraw = new Draw
    //     {
    //         Id = 2,
    //         MatchType = new Group { Name = "Group A" }
    //     };
    //     tournament.Draws = new List<Draw> { knockoutDraw, groupDraw };

    //     var expectedMatches = new List<Match> { CreateMatch(tournament), CreateMatch(tournament) };
    //     var gameFormatMock = new Mock<IGameFormat>();
    //     var playerStandings = new List<PlayerStanding> { new PlayerStanding() };

    //     _drawRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<string[]>()))
    //         .ReturnsAsync((int id, string[] props) => tournament.Draws.First(d => d.Id == id));
    //     _gameFormatFactoryMock.Setup(x => x.GetGameFormat(tournament.GameType.Name))
    //         .Returns(gameFormatMock.Object);
    //     gameFormatMock.Setup(x => x.GetGroupStanding(tournament, It.IsAny<Group>()))
    //         .Returns(playerStandings);
    //     _knockoutMock.Setup(x => x.CreateFirstRoundMatchesAfterGroup(
    //         tournament,
    //         knockoutDraw.MatchType,
    //         It.IsAny<Dictionary<string, List<PlayerStanding>>>()))
    //         .ReturnsAsync(expectedMatches);

    //     // Act
    //     var result = await _matchService.CreateMatches(tournament, null);

    //     // Assert
    //     Assert.Equal(expectedMatches.Count, result.Count());
    //     _knockoutMock.Verify(x => x.CreateFirstRoundMatchesAfterGroup(
    //         tournament,
    //         knockoutDraw.MatchType,
    //         It.IsAny<Dictionary<string, List<PlayerStanding>>>()),
    //         Times.Once);
    // }

    // [Fact]
    // public async Task IsMatchComplete_WithBothPlayersBye_ThrowsException()
    // {
    //     // Arrange
    //     var tournament = TournamentFixtures.GetTournament();
    //     var match = CreateMatch(tournament);
    //     match.FirstPlayer = new Player { Name = "bye" };
    //     match.SecondPlayer = new Player { Name = "BYE" };

    //     // Act & Assert
    //     await Assert.ThrowsAsync<Exception>(() => _matchService.IsMatchComplete(match));
    // }

    // [Fact]
    // public async Task IsMatchComplete_WithOnePlayerBye_ReturnsTrue()
    // {
    //     // Arrange
    //     var tournament = TournamentFixtures.GetTournament();
    //     var match = CreateMatch(tournament);
    //     match.FirstPlayer = PlayerFixtures.StandardPlayer;
    //     match.SecondPlayer = new Player { Name = "bye" };

    //     // Act
    //     var result = await _matchService.IsMatchComplete(match);

    //     // Assert
    //     Assert.True(result);
    // }

    // [Fact]
    // public async Task IsMatchComplete_WithScoreJson_ReturnsTrue()
    // {
    //     // Arrange
    //     var tournament = TournamentFixtures.GetTournament();
    //     var match = CreateMatch(tournament);
    //     match.FirstPlayer = PlayerFixtures.StandardPlayer;
    //     match.SecondPlayer = PlayerFixtures.HighRatedPlayer;
    //     match.ScoreJson = new { Score = "21-19" };

    //     // Act
    //     var result = await _matchService.IsMatchComplete(match);

    //     // Assert
    //     Assert.True(result);
    // }

    // [Fact]
    // public async Task IsMatchComplete_WithNoScoreAndNoByePlayers_ReturnsFalse()
    // {
    //     // Arrange
    //     var tournament = TournamentFixtures.GetTournament();
    //     var match = CreateMatch(tournament);
    //     match.FirstPlayer = PlayerFixtures.StandardPlayer;
    //     match.SecondPlayer = PlayerFixtures.HighRatedPlayer;

    //     // Act
    //     var result = await _matchService.IsMatchComplete(match);

    //     // Assert
    //     Assert.False(result);
    // }

    // [Fact]
    // public async Task IsMatchComplete_WithNullPlayers_LoadsPlayersAndChecksCompletion()
    // {
    //     // Arrange
    //     var tournament = TournamentFixtures.GetTournament();
    //     var match = CreateMatch(tournament);
    //     match.FirstPlayer = null!;
    //     match.SecondPlayer = null!;

    //     _matchRepositoryMock.Setup(x => x.ExplicitLoadReferenceAsync(match, It.IsAny<Expression<Func<Match, Player?>>>()))
    //         .Callback<Match, Expression<Func<Match, Player?>>>((m, exp) =>
    //         {
    //             if (exp.Body.ToString().Contains("FirstPlayer"))
    //                 m.FirstPlayer = PlayerFixtures.StandardPlayer;
    //             else
    //                 m.SecondPlayer = PlayerFixtures.HighRatedPlayer;
    //         });

    //     // Act
    //     var result = await _matchService.IsMatchComplete(match);

    //     // Assert
    //     Assert.False(result);
    //     _matchRepositoryMock.Verify(x => x.ExplicitLoadReferenceAsync(match, It.IsAny<Expression<Func<Match, Player?>>>()), Times.Exactly(2));
    // }

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
}