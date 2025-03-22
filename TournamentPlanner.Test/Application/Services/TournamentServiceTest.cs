using Moq;
using TournamentPlanner.Application;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.Helpers;
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
    private readonly Mock<IRepository<MatchType>> _matchTypeRepositoryMock;
    private readonly Mock<IRoundService> _roundServiceMock;
    private readonly TournamentService _sut;
    private readonly Mock<ICurrentUser> _currentUserMock;

    public TournamentServiceTest()
    {
        _drawServiceMock = new Mock<IDrawService>();
        _matchTypeServiceMock = new Mock<IMatchTypeService>();
        _tournamentRepositoryMock = new Mock<IRepository<Tournament>>();
        _matchTypeRepositoryMock = new Mock<IRepository<MatchType>>();
        _roundServiceMock = new Mock<IRoundService>();
        _currentUserMock = new Mock<ICurrentUser>();
        _sut = new TournamentService(
            _drawServiceMock.Object,
            _matchTypeServiceMock.Object,
            _tournamentRepositoryMock.Object,
            _roundServiceMock.Object,
            _currentUserMock.Object,
            _matchTypeRepositoryMock.Object
        );
    }

    [Fact]
    public async Task CanIMakeDraw_WithNoDraws_ReturnsTrue()
    {
        // Arrange
        var tournament = TournamentFixtures.GetGroupTournament();
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
        var matchType = Fixtures.MatchTypeFixtures.GetGroup();
        tournament.Draws = new List<Draw>
        {
            new Draw { MatchType = matchType, Tournament = tournament },
        };
        tournament.CurrentState = TournamentState.GroupState;

        _drawServiceMock.Setup(x => x.IsDrawsComplete(tournament)).ReturnsAsync(true);

        // Act
        var result = await _sut.CanIMakeDraw(tournament);

        // Assert
        Assert.True(result);
        _drawServiceMock.Verify(x => x.IsDrawsComplete(tournament), Times.Once);
    }

    [Fact]
    public async Task CanIMakeDraw_InKnockoutState_ReturnsFalse()
    {
        // Arrange
        var tournament = TournamentFixtures.GetKnockoutTournament();
        tournament.Draws = new List<Draw>
        {
            new Draw
            {
                Tournament = tournament,
                MatchType = new Group { Name = "Group A" },
            },
        };
        tournament.CurrentState = TournamentState.KnockoutState;

        // Act
        var result = await _sut.CanIMakeDraw(tournament);

        // Assert
        Assert.False(result);
        _drawServiceMock.Verify(x => x.IsDrawsComplete(tournament), Times.Never);
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
        tournament.Draws = new List<Draw>
        {
            new Draw
            {
                Tournament = tournament,
                MatchType = new Group { Name = "Group A" },
            },
        };
        tournament.Matches = new List<Match>();

        // Act
        var result = await _sut.CanISchedule(tournament);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CanISchedule_KnockoutState_WithNoFinalRound_ReturnFalse()
    {
        // Arrange
        var tournament = TournamentFixtures.GetKnockoutTournament();
        tournament.CurrentState = TournamentState.KnockoutState;
        var matches = Fixtures.MatchFixtures.GetMatches();
        var matchType = Fixtures.MatchTypeFixtures.GetKnockOut();
        var round = new Round
        {
            RoundName = "NotFinal",
            MatchType = matchType,
            Matches = matches,
        };
        matchType.Rounds = new List<Round> { round };
        var draw = new Draw { MatchType = matchType, Tournament = tournament };
        tournament.Draws = new List<Draw> { draw };
        tournament.Matches = matches;

        // Act
        var result = await _sut.CanISchedule(tournament);
        _roundServiceMock
            .Setup(x => x.IsAllRoundComplete(It.IsAny<MatchType>()))
            .ReturnsAsync(false);

        // Assert
        Assert.False(result);
        _roundServiceMock.Verify(x => x.IsAllRoundComplete(matchType), Times.Once);
    }

    [Fact]
    public async Task CanISchedule_KnockoutState_WithNoFinalRound_ReturnTrue()
    {
        // Arrange
        var tournament = TournamentFixtures.GetKnockoutTournament();
        tournament.CurrentState = TournamentState.KnockoutState;
        var matches = Fixtures.MatchFixtures.GetMatches();
        var matchType = Fixtures.MatchTypeFixtures.GetKnockOut();
        var round = new Round
        {
            RoundName = "NotFinal",
            MatchType = matchType,
            Matches = matches,
        };
        matchType.Rounds = new List<Round> { round };
        var draw = new Draw { MatchType = matchType, Tournament = tournament };
        tournament.Draws = new List<Draw> { draw };
        tournament.Matches = matches;

        // Act
        _roundServiceMock
            .Setup(x => x.IsAllRoundComplete(It.IsAny<MatchType>()))
            .ReturnsAsync(true);
        var result = await _sut.CanISchedule(tournament);

        // Assert
        _roundServiceMock.Verify(x => x.IsAllRoundComplete(matchType), Times.Once);
        Assert.True(result);
    }

    [Fact]
    public async Task CanISchedule_KnockoutState_WithFinalRound_ReturnFalse()
    {
        // Arrange
        var tournament = TournamentFixtures.GetKnockoutTournament();
        tournament.CurrentState = TournamentState.KnockoutState;
        var matches = Fixtures.MatchFixtures.GetMatches();
        var matchType = Fixtures.MatchTypeFixtures.GetKnockOut();
        var round = new Round
        {
            RoundName = Utility.Final,
            MatchType = matchType,
            Matches = matches,
        };
        matchType.Rounds = new List<Round> { round };
        var draw = new Draw { MatchType = matchType, Tournament = tournament };
        tournament.Draws = new List<Draw> { draw };
        tournament.Matches = matches;

        // Act
        var result = await _sut.CanISchedule(tournament);

        // Assert
        Assert.False(result);
        _roundServiceMock.Verify(x => x.IsAllRoundComplete((It.IsAny<MatchType>())), Times.Never);
    }

    [Fact]
    public async Task MakeDraws_WithValidSeeders_ReturnsDraws()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        tournament.Participants.AddRange(PlayerFixtures.GetSamplePlayers(2));

        var seeders = new List<int> { 1 };

        var matchType = new Group { Name = "Group A" };
        _matchTypeServiceMock
            .Setup(x => x.CreateMatchType(tournament, It.IsAny<string>(), seeders))
            .ReturnsAsync(new List<MatchType> { matchType });

        // Act
        var result = await _sut.MakeDraws(tournament, null, seeders);

        // Assert
        Assert.Single(result);
        Assert.Equal(matchType, result.First().MatchType);
    }

    [Fact]
    public async Task MakeDraws_VerifyThatTournamentStatusChange_ReturnChangedStatus()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        tournament.Participants.AddRange(PlayerFixtures.GetSamplePlayers(2));

        var seeders = new List<int> { 1 };

        var matchType = new Group { Name = "Group A" };
        _matchTypeServiceMock
            .Setup(x => x.CreateMatchType(tournament, It.IsAny<string>(), seeders))
            .ReturnsAsync(new List<MatchType> { matchType });

        // Act
        var result = await _sut.MakeDraws(tournament, null, seeders);

        // Assert
        Assert.Single(result);
        Assert.Equal(tournament.Status, TournamentStatus.Ongoing); //expected the changed status
        Assert.Equal(matchType, result.First().MatchType);
    }

    [Fact]
    public async Task MakeDraws_WithNullTournament_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.MakeDraws(null!, null));
    }

    [Fact]
    public async Task MakeDraws_WithInvalidSeeders_ThrowsException()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        tournament.Participants.AddRange(PlayerFixtures.GetSamplePlayers(2));

        var seeders = new List<int> { 3 }; // Invalid seeder

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _sut.MakeDraws(tournament, null, seeders));
    }

    [Fact]
    public void AmICreator_WithNullCurrentUser_ReturnsFalse()
    {
        // Arrange
        var tournament = Fixtures.TournamentFixtures.GetTournament();
        tournament.AdminId = 1;
        _currentUserMock.Setup(x => x.DomainUserId).Returns((int?)null);

        // Act
        var result = _sut.AmITheCreator(tournament);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void AmICreator_WithNullTournament_ReturnsFalse()
    {
        // Arrange
        _currentUserMock.Setup(x => x.DomainUserId).Returns(1);

        // Act
        var result = _sut.AmITheCreator(null!);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void AmICreator_WithAdminIdZero_ReturnsFalse()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        _currentUserMock.Setup(x => x.DomainUserId).Returns(1);

        // Act
        var result = _sut.AmITheCreator(tournament);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void AmICreator_WithDifferentAdminId_ReturnsFalse()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        tournament.AdminId = tournament.CreatedBy.Id;
        _currentUserMock.Setup(x => x.DomainUserId).Returns(2);

        // Act
        var result = _sut.AmITheCreator(tournament);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void AmICreator_WithSameAdminId_ReturnsTrue()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        tournament.AdminId = tournament.CreatedBy.Id;

        _currentUserMock.Setup(x => x.DomainUserId).Returns(tournament.CreatedBy.Id);

        // Act
        var result = _sut.AmITheCreator(tournament);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ChangeTournamentStatus_NullTournament_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _sut.ChangeTournamentStatus((Tournament)null!, TournamentStatus.Draft)
        );
    }

    [Fact]
    public async Task ChangeTournamentStatus_FromDraftToRegistrationOpen_ReturnsTrue()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        tournament.Status = TournamentStatus.Draft;

        // Act
        var changeTournamentStatusResult = await _sut.ChangeTournamentStatus(
            tournament,
            TournamentStatus.RegistrationOpen
        );

        // Assert
        Assert.True(changeTournamentStatusResult.Success);
        Assert.Equal(TournamentStatus.RegistrationOpen, tournament.Status);
        _tournamentRepositoryMock.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task ChangeTournamentStatus_FromRegistrationOpenToDraft_ReturnsTrue()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        tournament.Status = TournamentStatus.RegistrationOpen;

        // Act
        var changeTournamentStatusResult = await _sut.ChangeTournamentStatus(
            tournament,
            TournamentStatus.Draft
        );

        // Assert
        Assert.True(changeTournamentStatusResult.Success);
        Assert.Equal(TournamentStatus.Draft, tournament.Status);
        _tournamentRepositoryMock.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task ChangeTournamentStatus_FromOngoingToLowerStatus_ReturnsFalse()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        tournament.Status = TournamentStatus.Ongoing;

        // Act
        var changeTournamentStatusResult = await _sut.ChangeTournamentStatus(
            tournament,
            TournamentStatus.RegistrationClosed
        );

        // Assert
        Assert.False(changeTournamentStatusResult.Success);
        Assert.Equal(TournamentStatus.Ongoing, tournament.Status);
        _tournamentRepositoryMock.Verify(r => r.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task ChangeTournamentStatus_FromOngoingToCompleted_ReturnsTrue()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        tournament.Status = TournamentStatus.Ongoing;

        // Act
        var changeTournamentStatusResult = await _sut.ChangeTournamentStatus(
            tournament,
            TournamentStatus.Completed
        );

        // Assert
        Assert.True(changeTournamentStatusResult.Success);
        Assert.Equal(TournamentStatus.Completed, tournament.Status);
        _tournamentRepositoryMock.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task ChangeTournamentStatus_FromCompletedToAnyStatus_ReturnsFalse()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        tournament.Status = TournamentStatus.Completed;

        // Act
        var changeTournamentStatusResult = await _sut.ChangeTournamentStatus(
            tournament,
            TournamentStatus.Ongoing
        );

        // Assert
        Assert.False(changeTournamentStatusResult.Success);
        Assert.Equal(TournamentStatus.Completed, tournament.Status);
        _tournamentRepositoryMock.Verify(r => r.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task ChangeTournamentStatus_WithStringId_ValidId_ReturnsTrue()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        tournament.Status = TournamentStatus.RegistrationClosed;
        _tournamentRepositoryMock
            .Setup(r => r.GetByIdAsync(tournament.Id))
            .ReturnsAsync(tournament);

        // Act
        var changeTournamentStatusResult = await _sut.ChangeTournamentStatus(
            tournament.Id.ToString(),
            TournamentStatus.Ongoing
        );

        // Assert
        Assert.True(changeTournamentStatusResult.Success);
        Assert.Equal(TournamentStatus.Ongoing, tournament.Status);
        _tournamentRepositoryMock.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task ChangeTournamentStatus_WithStringId_InvalidIdFormat_ThrowsArgumentException()
    {
        // Arrange
        var invalidId = "invalid-id";

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.ChangeTournamentStatus(invalidId, TournamentStatus.Ongoing)
        );

        _tournamentRepositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
        _tournamentRepositoryMock.Verify(r => r.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task ChangeTournamentStatus_WithStringId_TournamentNotFound_ThrowsArgumentNullException()
    {
        // Arrange
        var tournamentId = "42";
        _tournamentRepositoryMock.Setup(r => r.GetByIdAsync(42)).ReturnsAsync((Tournament)null!);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _sut.ChangeTournamentStatus(tournamentId, TournamentStatus.Ongoing)
        );

        _tournamentRepositoryMock.Verify(r => r.GetByIdAsync(42), Times.Once);
        _tournamentRepositoryMock.Verify(r => r.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task ChangeTournamentStatus_WithStringId_NullOrEmptyId_ThrowsArgumentNullException()
    {
        // Act & Assert
        //ArgumentNullException.ThrowIfNullOrEmpty(tournamentId) => this will throw ArgumentException not ArgumentNullException
        //if a Empty string is passed
        await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.ChangeTournamentStatus(String.Empty, TournamentStatus.Ongoing)
        );
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _sut.ChangeTournamentStatus((string)null!, TournamentStatus.Ongoing)
        );

        _tournamentRepositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
        _tournamentRepositoryMock.Verify(r => r.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task ChangeTournamentStatus_RequestedStatusSameAsCurrentStatus_ReturnsTrueWithoutSaving()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        tournament.Status = TournamentStatus.RegistrationOpen;

        // Act
        var changeTournamentStatusResult = await _sut.ChangeTournamentStatus(
            tournament,
            TournamentStatus.RegistrationOpen
        );

        // Assert
        Assert.True(changeTournamentStatusResult.Success);
        Assert.Equal(TournamentStatus.RegistrationOpen, tournament.Status);
        _tournamentRepositoryMock.Verify(r => r.SaveAsync(), Times.Never);
    }
}
