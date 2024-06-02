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

}