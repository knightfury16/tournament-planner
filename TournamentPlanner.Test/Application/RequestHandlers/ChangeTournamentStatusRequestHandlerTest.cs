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
    private readonly Mock<IRepository<Tournament>> _mockTournamentRepository;
    private readonly Mock<ITournamentService> _mockTournamentService;
    private readonly ChangeTournamentStatusRequestHandler _handler;


    public ChangeTournamentStatusRequestHandlerTest()
    {
        _mockTournamentRepository = new Mock<IRepository<Tournament>>();
        _mockTournamentService = new Mock<ITournamentService>();
        _handler = new ChangeTournamentStatusRequestHandler(_mockTournamentRepository.Object, _mockTournamentService.Object);
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
        var request = new ChangeTournamentStatusRequest(tournamentId, TournamentStatus.Draft.ToString());
        _mockTournamentRepository.Setup(r => r.GetByIdAsync(tournamentId)).ReturnsAsync((Tournament)null!);

        // Act and Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ChangeStatusToOngoing_ReturnsTrue()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        tournament.Status = TournamentStatus.RegistrationClosed;
        var request = new ChangeTournamentStatusRequest(tournament.Id, TournamentStatus.Ongoing.ToString());
        _mockTournamentRepository.Setup(r => r.GetByIdAsync(tournament.Id)).ReturnsAsync(tournament);
        _mockTournamentService.Setup(s => s.AmITheCreator(tournament)).Returns(true);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result);
        _mockTournamentRepository.Verify(r => r.SaveAsync(), Times.Once);
        _mockTournamentService.Verify(r => r.AmITheCreator(tournament), Times.Once);
    }

    [Fact]
    public async Task Handle_ChangeStatusToCompleted_ReturnsTrue()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        tournament.Status = TournamentStatus.Ongoing;
        var request = new ChangeTournamentStatusRequest(tournament.Id, TournamentStatus.Completed.ToString());
        _mockTournamentRepository.Setup(r => r.GetByIdAsync(tournament.Id)).ReturnsAsync(tournament);
        _mockTournamentService.Setup(s => s.AmITheCreator(tournament)).Returns(true);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result);
        _mockTournamentRepository.Verify(r => r.SaveAsync(), Times.Once);
        _mockTournamentService.Verify(r => r.AmITheCreator(tournament), Times.Once);
    }

    [Fact]
    public async Task Handle_ChangeStatusToLowerAfterOngoig_ReturnsFalse()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        tournament.Status = TournamentStatus.Ongoing;
        //request lower to draft
        var request = new ChangeTournamentStatusRequest(tournament.Id, TournamentStatus.Draft.ToString());
        _mockTournamentRepository.Setup(r => r.GetByIdAsync(tournament.Id)).ReturnsAsync(tournament);
        _mockTournamentService.Setup(s => s.AmITheCreator(tournament)).Returns(true);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result);
        _mockTournamentRepository.Verify(r => r.SaveAsync(), Times.Never);
        _mockTournamentService.Verify(r => r.AmITheCreator(tournament), Times.Once);

        // Arrange requst lower to RegistrationOpen
        request = new ChangeTournamentStatusRequest(tournament.Id, TournamentStatus.RegistrationOpen.ToString());
        //Act and Assert
        result = await _handler.Handle(request, CancellationToken.None);
        Assert.False(result);
        _mockTournamentRepository.Verify(r => r.SaveAsync(), Times.Never);
        _mockTournamentService.Verify(r => r.AmITheCreator(tournament), Times.AtLeastOnce);

        // Arrange requst lower to RegistrationClosed
        request = new ChangeTournamentStatusRequest(tournament.Id, TournamentStatus.RegistrationClosed.ToString());
        //Act and Assert
        result = await _handler.Handle(request, CancellationToken.None);
        Assert.False(result);
        _mockTournamentRepository.Verify(r => r.SaveAsync(), Times.Never);
        _mockTournamentService.Verify(r => r.AmITheCreator(tournament), Times.AtLeastOnce);

        // Arrange requst lower and equal to Ongoing
        request = new ChangeTournamentStatusRequest(tournament.Id, TournamentStatus.Ongoing.ToString());
        //Act and Assert
        result = await _handler.Handle(request, CancellationToken.None);
        Assert.False(result);
        _mockTournamentRepository.Verify(r => r.SaveAsync(), Times.Never);
        _mockTournamentService.Verify(r => r.AmITheCreator(tournament), Times.AtLeastOnce);

    }
    [Fact]
    public async Task Handle_ChangeStatusToHigerAfterOngoig_ReturnsTrue()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        tournament.Status = TournamentStatus.Ongoing;
        //request higer to complete
        var request = new ChangeTournamentStatusRequest(tournament.Id, TournamentStatus.Completed.ToString());
        _mockTournamentRepository.Setup(r => r.GetByIdAsync(tournament.Id)).ReturnsAsync(tournament);
        _mockTournamentService.Setup(s => s.AmITheCreator(tournament)).Returns(true);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result);
        _mockTournamentService.Verify(r => r.AmITheCreator(tournament), Times.Once);

    }

    [Fact]
    public async Task Handle_ChangeStatusToLowerAfterComplete_ReturnsFalse()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        tournament.Status = TournamentStatus.Completed;
        //request lower to draft
        var request = new ChangeTournamentStatusRequest(tournament.Id, TournamentStatus.Draft.ToString());
        _mockTournamentRepository.Setup(r => r.GetByIdAsync(tournament.Id)).ReturnsAsync(tournament);
        _mockTournamentService.Setup(s => s.AmITheCreator(tournament)).Returns(true);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result);
        _mockTournamentRepository.Verify(r => r.SaveAsync(), Times.Never);
        _mockTournamentService.Verify(r => r.AmITheCreator(tournament), Times.Once);

        // Arrange requst lower to RegistrationOpen
        request = new ChangeTournamentStatusRequest(tournament.Id, TournamentStatus.RegistrationOpen.ToString());
        //Act and Assert
        result = await _handler.Handle(request, CancellationToken.None);
        Assert.False(result);
        _mockTournamentRepository.Verify(r => r.SaveAsync(), Times.Never);
        _mockTournamentService.Verify(r => r.AmITheCreator(tournament), Times.AtLeastOnce);

        // Arrange requst lower to RegistrationClosed
        request = new ChangeTournamentStatusRequest(tournament.Id, TournamentStatus.RegistrationClosed.ToString());
        //Act and Assert
        result = await _handler.Handle(request, CancellationToken.None);
        Assert.False(result);
        _mockTournamentRepository.Verify(r => r.SaveAsync(), Times.Never);
        _mockTournamentService.Verify(r => r.AmITheCreator(tournament), Times.AtLeastOnce);

        // Arrange requst lower to Ongoing
        request = new ChangeTournamentStatusRequest(tournament.Id, TournamentStatus.Ongoing.ToString());
        //Act and Assert
        result = await _handler.Handle(request, CancellationToken.None);
        Assert.False(result);
        _mockTournamentRepository.Verify(r => r.SaveAsync(), Times.Never);
        _mockTournamentService.Verify(r => r.AmITheCreator(tournament), Times.AtLeastOnce);

        // Arrange requst lower and equal to  completed
        request = new ChangeTournamentStatusRequest(tournament.Id, TournamentStatus.Completed.ToString());
        //Act and Assert
        result = await _handler.Handle(request, CancellationToken.None);
        Assert.False(result);
        _mockTournamentRepository.Verify(r => r.SaveAsync(), Times.Never);
        _mockTournamentService.Verify(r => r.AmITheCreator(tournament), Times.AtLeastOnce);

    }
    [Fact]
    public async Task Handle_NotTournamentCreator_ThrowsAdminOwnershipException()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        var request = new ChangeTournamentStatusRequest(tournament.Id, TournamentStatus.RegistrationOpen.ToString());
        _mockTournamentRepository.Setup(r => r.GetByIdAsync(tournament.Id)).ReturnsAsync(tournament);
        _mockTournamentService.Setup(s => s.AmITheCreator(tournament)).Returns(false);

        // Act & Assert
        await Assert.ThrowsAsync<AdminOwnershipException>(() => 
            _handler.Handle(request, CancellationToken.None));

        _mockTournamentRepository.Verify(r => r.SaveAsync(), Times.Never);
    }
}
