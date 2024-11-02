using Moq;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Exceptions;
using TournamentPlanner.Test.Fixtures;
using System.Linq.Expressions;

namespace TournamentPlanner.Application.UnitTests.Services;

public class DrawServiceTests
{
    private readonly Mock<IRepository<Draw>> _mockDrawRepository;
    private readonly DrawService _sut;

    public DrawServiceTests()
    {
        _mockDrawRepository = new Mock<IRepository<Draw>>();
        _sut = new DrawService(_mockDrawRepository.Object);
    }

    [Fact]
    public async Task IsDrawsComplete_Tournament_WithAllCompletedDraws_ReturnsTrue()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        var draws = new List<Draw>
        {
            new Draw { Tournament = tournament, MatchType = new Group { Name = "Group A", IsCompleted = true } },
            new Draw { Tournament = tournament, MatchType = new Group { Name = "Group B", IsCompleted = true } }
        };

        _mockDrawRepository.Setup(x => x.GetAllAsync(
            It.IsAny<Expression<Func<Draw, bool>>>(),
            It.IsAny<string[]>()))
            .ReturnsAsync(draws);

        // Act
        var result = await _sut.IsDrawsComplete(tournament);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task IsDrawsComplete_Tournament_WithIncompleteDraws_ReturnsFalse()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        var draws = new List<Draw>
        {
            new Draw { Tournament = tournament, MatchType = new Group {Name = "Group A", IsCompleted = true } },
            new Draw { Tournament = tournament, MatchType = new Group {Name = "Group B", IsCompleted = false } }
        };

        _mockDrawRepository.Setup(x => x.GetAllAsync(
            It.IsAny<Expression<Func<Draw, bool>>>(),
            It.IsAny<string[]>()))
            .ReturnsAsync(draws);

        // Act
        var result = await _sut.IsDrawsComplete(tournament);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task IsDrawsComplete_Draws_WithAllCompletedDraws_ReturnsTrue()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        var draws = new List<Draw>
        {
            new Draw { Tournament = tournament, MatchType = new Group { Name = "Group A", IsCompleted = true } },
            new Draw { Tournament = tournament, MatchType = new Group { Name = "Group B", IsCompleted = true } }
        };

        // Act
        var result = await _sut.IsDrawsComplete(draws);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task IsDrawsComplete_Draws_WithIncompleteDraws_ReturnsFalse()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        var draws = new List<Draw>
        {
            new Draw { Tournament = tournament, MatchType = new Group { Name = "Group A", IsCompleted = true } },
            new Draw { Tournament = tournament, MatchType = new Group { Name = "Group B", IsCompleted = false } }
        };

        // Act
        var result = await _sut.IsDrawsComplete(draws);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task IsDrawsComplete_Draws_WithNullDraws_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.IsDrawsComplete((IEnumerable<Draw>)null!));
    }

    [Fact]
    public async Task IsDrawsComplete_Draws_WithNullMatchType_ThrowsNotFoundException()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        var draws = new List<Draw>
        {
            new Draw { Tournament = tournament, MatchType = new Group { Name = "Group A", IsCompleted = true } },
            new Draw {Tournament = tournament,  MatchType = null! }
        };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _sut.IsDrawsComplete(draws));
    }
}
