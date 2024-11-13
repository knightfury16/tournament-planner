using Moq;
using TournamentPlanner.Application;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Enum;
using TournamentPlanner.Domain.Exceptions;
using TournamentPlanner.Test.Fixtures;

namespace TournamentPlanner.Test.Application.RequestHandlers;

public class ChangeTournamentStatusRequestHandlerTest
{
    private readonly Mock<IRepository<Tournament>> _mockRepository;
    private readonly ChangeTournamentStatusRequestHandler _handler;

    public ChangeTournamentStatusRequestHandlerTest()
    {
        _mockRepository = new Mock<IRepository<Tournament>>();
        _handler = new ChangeTournamentStatusRequestHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_NullRequest_ThrowsArgumentNullException()
    {
        // Act and Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Handle(null!, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_TournamentNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var tournamentId = 1;
        var request = new ChangeTournamentStatusRequest(tournamentId, TournamentStatus.Draft);
        _mockRepository.Setup(r => r.GetByIdAsync(tournamentId)).ReturnsAsync((Tournament)null!);

        // Act and Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ChangeStatusToOngoing_ReturnsTrue()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        tournament.Status = TournamentStatus.RegistrationClosed;
        var request = new ChangeTournamentStatusRequest(tournament.Id, TournamentStatus.Ongoing);
        _mockRepository.Setup(r => r.GetByIdAsync(tournament.Id)).ReturnsAsync(tournament);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result);
        _mockRepository.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ChangeStatusToCompleted_ReturnsTrue()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        tournament.Status = TournamentStatus.Ongoing;
        var request = new ChangeTournamentStatusRequest(tournament.Id, TournamentStatus.Completed);
        _mockRepository.Setup(r => r.GetByIdAsync(tournament.Id)).ReturnsAsync(tournament);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result);
        _mockRepository.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ChangeStatusToLowerAfterOngoig_ReturnsFalse()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        tournament.Status = TournamentStatus.Ongoing;
        //request lower to draft
        var request = new ChangeTournamentStatusRequest(tournament.Id, TournamentStatus.Draft);
        _mockRepository.Setup(r => r.GetByIdAsync(tournament.Id)).ReturnsAsync(tournament);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result);
        _mockRepository.Verify(r => r.SaveAsync(), Times.Never);

        // Arrange requst lower to RegistrationOpen
        request = new ChangeTournamentStatusRequest(tournament.Id, TournamentStatus.RegistrationOpen);
        //Act and Assert
        result = await _handler.Handle(request, CancellationToken.None);
        Assert.False(result);
        _mockRepository.Verify(r => r.SaveAsync(), Times.Never);

        // Arrange requst lower to RegistrationClosed
        request = new ChangeTournamentStatusRequest(tournament.Id, TournamentStatus.RegistrationClosed);
        //Act and Assert
        result = await _handler.Handle(request, CancellationToken.None);
        Assert.False(result);
        _mockRepository.Verify(r => r.SaveAsync(), Times.Never);

        // Arrange requst lower and equal to Ongoing
        request = new ChangeTournamentStatusRequest(tournament.Id, TournamentStatus.Ongoing);
        //Act and Assert
        result = await _handler.Handle(request, CancellationToken.None);
        Assert.False(result);
        _mockRepository.Verify(r => r.SaveAsync(), Times.Never);

    }
    [Fact]
    public async Task Handle_ChangeStatusToHigerAfterOngoig_ReturnsTrue()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        tournament.Status = TournamentStatus.Ongoing;
        //request higer to complete
        var request = new ChangeTournamentStatusRequest(tournament.Id, TournamentStatus.Completed);
        _mockRepository.Setup(r => r.GetByIdAsync(tournament.Id)).ReturnsAsync(tournament);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result);

    }

    [Fact]
    public async Task Handle_ChangeStatusToLowerAfterComplete_ReturnsFalse()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        tournament.Status = TournamentStatus.Completed;
        //request lower to draft
        var request = new ChangeTournamentStatusRequest(tournament.Id, TournamentStatus.Draft);
        _mockRepository.Setup(r => r.GetByIdAsync(tournament.Id)).ReturnsAsync(tournament);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result);
        _mockRepository.Verify(r => r.SaveAsync(), Times.Never);

        // Arrange requst lower to RegistrationOpen
        request = new ChangeTournamentStatusRequest(tournament.Id, TournamentStatus.RegistrationOpen);
        //Act and Assert
        result = await _handler.Handle(request, CancellationToken.None);
        Assert.False(result);
        _mockRepository.Verify(r => r.SaveAsync(), Times.Never);

        // Arrange requst lower to RegistrationClosed
        request = new ChangeTournamentStatusRequest(tournament.Id, TournamentStatus.RegistrationClosed);
        //Act and Assert
        result = await _handler.Handle(request, CancellationToken.None);
        Assert.False(result);
        _mockRepository.Verify(r => r.SaveAsync(), Times.Never);

        // Arrange requst lower to Ongoing
        request = new ChangeTournamentStatusRequest(tournament.Id, TournamentStatus.Ongoing);
        //Act and Assert
        result = await _handler.Handle(request, CancellationToken.None);
        Assert.False(result);
        _mockRepository.Verify(r => r.SaveAsync(), Times.Never);

        // Arrange requst lower and equal to  completed
        request = new ChangeTournamentStatusRequest(tournament.Id, TournamentStatus.Completed);
        //Act and Assert
        result = await _handler.Handle(request, CancellationToken.None);
        Assert.False(result);
        _mockRepository.Verify(r => r.SaveAsync(), Times.Never);

    }
}
