using Moq;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.UseCases.MatchUseCase;
using DomainMatch = TournamentPlanner.Domain.Entities.Match;
using TournamentPlanner.Domain.Entities;


namespace TournamentPlanner.Test.Application;

public class MatchUseCaseTest
{
    private readonly Mock<IRepository<DomainMatch, DomainMatch>> matchRepositoryMock;
    private readonly MatchUseCase matchUseCase;

    public MatchUseCaseTest()
    {
        matchRepositoryMock = new Mock<IRepository<DomainMatch, DomainMatch>>();

        matchUseCase = new MatchUseCase(matchRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAllRoundMatches_Should_ReturnMatchesForRound()
    {
        // Arrange
        int roundId = 1;
        var matches = new List<DomainMatch>
            {
                new DomainMatch { RoundId = roundId, FirstPlayer = new Player(), SecondPlayer = new Player() },
                new DomainMatch { RoundId = roundId, FirstPlayer = new Player(), SecondPlayer = new Player() }
            };

        matchRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Func<DomainMatch, bool>>(), It.IsAny<string[]>())).ReturnsAsync(matches);

        // Act
        var result = await matchUseCase.GetAllRoundMatches(roundId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetAllMatches_Should_ReturnAllMatchesOfAllTournament()
    {
        //Arrange
        var matches = new List<DomainMatch>
            {
                new DomainMatch { RoundId = 1, FirstPlayer = new Player(), SecondPlayer = new Player() },
                new DomainMatch { RoundId = 2, FirstPlayer = new Player(), SecondPlayer = new Player() }
            };
        matchRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<string[]>())).ReturnsAsync(matches);

        //Act

        var result = await matchUseCase.GetAllMatches();

        //Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        matchRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<string[]>()), Times.Once());
    }

    [Fact]
    public async Task GetAllTournamentMatches_Should_ReturnMatchesForTournament()
    {
        // Arrange
        int tournamentId = 1;
        var matches = new List<DomainMatch>
            {
                new DomainMatch { Round = new Round { TournamentId = tournamentId }, FirstPlayer = new Player(), SecondPlayer = new Player() },
                new DomainMatch { Round = new Round { TournamentId = tournamentId }, FirstPlayer = new Player(), SecondPlayer = new Player() }
            };
        matchRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Func<DomainMatch, bool>>(), It.IsAny<string[]>())).ReturnsAsync(matches);

        // Act
        var result = await matchUseCase.GetAllTournamentMatches(tournamentId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetAllWinnersOfRound_Should_ReturnWinnersForRound()
    {
        // Arrange
        int roundId = 1;
        var matches = new List<DomainMatch>
            {
                new DomainMatch { RoundId = roundId, IsComplete = true, Winner = new Player() },
                new DomainMatch { RoundId = roundId, IsComplete = true, Winner = new Player() }
            };
        matchRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Func<DomainMatch, bool>>(), It.IsAny<string[]>())).ReturnsAsync(matches);

        // Act
        var result = await matchUseCase.GetAllWinnersOfRound(roundId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetOpenMatches_Should_ReturnOpenMatches()
    {
        // Arrange
        int roundId = 1;
        int tournamentId = 1;
        var matches = new List<DomainMatch>
            {
                new DomainMatch { RoundId = roundId, Round = new Round { TournamentId = tournamentId }, IsComplete = false, FirstPlayer = new Player(), SecondPlayer = new Player() },
                new DomainMatch { RoundId = roundId, Round = new Round { TournamentId = tournamentId }, IsComplete = false, FirstPlayer = new Player(), SecondPlayer = new Player() }
            };
        matchRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<IEnumerable<Func<DomainMatch, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(matches);

        // Act
        var result = await matchUseCase.GetOpenMatches(roundId, tournamentId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetPlayedMatches_Should_ReturnPlayedMatches()
    {
        // Arrange
        int roundId = 1;
        int tournamentId = 1;
        var matches = new List<DomainMatch>
            {
                new DomainMatch { RoundId = roundId, Round = new Round { TournamentId = tournamentId }, IsComplete = true, FirstPlayer = new Player(), SecondPlayer = new Player() },
                new DomainMatch { RoundId = roundId, Round = new Round { TournamentId = tournamentId }, IsComplete = true, FirstPlayer = new Player(), SecondPlayer = new Player() }
            };
        matchRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<IEnumerable<Func<DomainMatch, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(matches);

        // Act
        var result = await matchUseCase.GetPlayedMatches(roundId, tournamentId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetWinnerOfMatch_Should_ReturnWinnerOfMatch()
    {
        // Arrange
        int matchId = 1;
        var match = new List<DomainMatch>
            {
                new DomainMatch { Id = matchId, IsComplete = true, Winner = new Player() }
            };
        matchRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Func<DomainMatch, bool>>(), It.IsAny<string[]>())).ReturnsAsync(match);

        // Act
        var result = await matchUseCase.GetWinnerOfMatch(matchId);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task RescheduleAMatch_Should_RescheduleMatch()
    {
        // Arrange
        int matchId = 1;
        DateTime rescheduledDate = DateTime.Now.AddDays(1);
        var match = new DomainMatch { Id = matchId, GameScheduled = DateTime.Now };

        matchRepositoryMock.Setup(x => x.GetByIdAsync(matchId)).ReturnsAsync(match);

        // Act
        var result = await matchUseCase.RescheduleAMatch(matchId, rescheduledDate);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(rescheduledDate, result.GameScheduled);
        matchRepositoryMock.Verify(x => x.SaveAsync(), Times.Once());
    }

}