using System.Linq.Expressions;
using Moq;
using TournamentPlanner.Application;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Enum;
using TournamentPlanner.Domain.Exceptions;

namespace TournamentPlanner.Test.Application.RequestHandlers
{
    public class RegisterPlayerInTournamentRequestHandlerTests
    {
        private readonly Mock<IRepository<Tournament>> _mockTournamentRepository;
        private readonly Mock<IRepository<Player>> _mockPlayerRepository;
        private readonly Mock<ICurrentUser> _mockCurrentUser;
        private readonly RegisterPlayerInTournamentRequestHandler _handler;

        public RegisterPlayerInTournamentRequestHandlerTests()
        {
            _mockTournamentRepository = new Mock<IRepository<Tournament>>();
            _mockPlayerRepository = new Mock<IRepository<Player>>();
            _mockCurrentUser = new Mock<ICurrentUser>();
            _handler = new RegisterPlayerInTournamentRequestHandler(
                _mockTournamentRepository.Object,
                _mockPlayerRepository.Object, _mockCurrentUser.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsTrue()
        {
            // Arrange
            var tournament = GetSomeMockTournaments().First();
            var beforeParticipants = tournament.Participants.Count;
            var player = new Player { Id = 1, Age = 20, Name = "test", Email = "test@gmail.com" };
            var registrationDto = new RegistrationInTournamentDto { TournamentId = tournament.Id };
            var request = new RegisterPlayerInTournamentRequest(registrationDto);
            _mockTournamentRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<string[]>())).ReturnsAsync(tournament);
            _mockPlayerRepository.Setup(r => r.GetByIdAsync(player.Id)).ReturnsAsync(player);
            _mockCurrentUser.Setup(u => u.DomainUserId).Returns(player.Id); // Set up the current user

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result);
            Assert.Equal(beforeParticipants +  1, tournament.Participants.Count);
            _mockTournamentRepository.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_NullRequest_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Handle(null!, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_TournamentNotFound_ThrowsInvalidOperationException()
        {
            // Arrange
            var registrationDto = new RegistrationInTournamentDto { TournamentId = 999 };
            var request = new RegisterPlayerInTournamentRequest(registrationDto);

            _mockTournamentRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Tournament)null!);

            // Act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(request, CancellationToken.None));

            // Assert
            Assert.Equal("Tournament not found", exception.Message);
        }


        [Fact]
        public async Task Handle_TournamentNotOpenForRegistration_ThrowsInvalidOperationException()
        {
            // Arrange
            var tournament = GetSomeMockTournaments().First(t => t.Status != TournamentStatus.RegistrationOpen);
            var registrationDto = new RegistrationInTournamentDto { TournamentId = tournament.Id};
            var request = new RegisterPlayerInTournamentRequest(registrationDto);

            _mockTournamentRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<string[]>())).ReturnsAsync(tournament);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(request, CancellationToken.None));
            Assert.Equal("Tournament Registration is not open", exception.Message);
        }
        [Fact]
        public async Task Handle_RegistrationDeadlinePassed_ThrowsInvalidOperationException()
        {
            // Arrange
            var tournament = GetSomeMockTournaments().FirstOrDefault(t => t.RegistrationLastDate.HasValue && t.RegistrationLastDate < DateTime.UtcNow);
            var registrationDto = new RegistrationInTournamentDto { TournamentId = tournament!.Id };
            var request = new RegisterPlayerInTournamentRequest(registrationDto);

            _mockTournamentRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<string[]>())).ReturnsAsync(tournament);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(request, CancellationToken.None));
            Assert.Equal("Registration deadline has passed", exception.Message);
        }


        [Fact]
        public async Task Handle_TournamentFull_ThrowsInvalidOperationException()
        {
            // Arrange
            var tournament = GetSomeMockTournaments().First(t => t.Participants.Count >= t.MaxParticipant);
            var registrationDto = new RegistrationInTournamentDto { TournamentId = tournament.Id };
            var request = new RegisterPlayerInTournamentRequest(registrationDto);

            _mockTournamentRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<string[]>())).ReturnsAsync(tournament);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(request, CancellationToken.None));
            Assert.Equal("Tournament has reached the maximum number of participants", exception.Message);
        }


        [Fact]
        public async Task Handle_PlayerNotFound_ThrowsInvalidOperationException()
        {
            // Arrange
            var tournament = GetSomeMockTournaments().First(t => t.Status == TournamentStatus.RegistrationOpen);
            var registrationDto = new RegistrationInTournamentDto { TournamentId = tournament.Id };
            var request = new RegisterPlayerInTournamentRequest(registrationDto);

            _mockTournamentRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>(),It.IsAny<string[]>())).ReturnsAsync(tournament);
            _mockCurrentUser.Setup(u => u.DomainUserId).Returns((int?)null); // Set up the current user to return null

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None)); // will assert only exception type not messages
        }


        [Fact]
        public async Task Handle_PlayerUnderAge_ThrowsValidationException()
        {
            // Arrange
            var tournament = GetSomeMockTournaments().First(t => t.MinimumAgeOfRegistration > 0);
            var player = new Player { Id = 1, Age = tournament.MinimumAgeOfRegistration - 1, Name = "test", Email = "test@gmail.com" };
            var registrationDto = new RegistrationInTournamentDto { TournamentId = tournament.Id };
            var request = new RegisterPlayerInTournamentRequest(registrationDto);

            _mockTournamentRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>(),It.IsAny<string[]>())).ReturnsAsync(tournament);
            _mockCurrentUser.Setup(u => u.DomainUserId).Returns(player.Id);
            _mockPlayerRepository.Setup(r => r.GetByIdAsync(player.Id)).ReturnsAsync(player);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(request, CancellationToken.None));
            Assert.Equal($"Player does not meet the minimum age requirement for {tournament.MinimumAgeOfRegistration}", exception.Message);
        }
        [Fact]
        public async Task Handle_PlayerAlreadyRegistered_ThrowsBadRequestException()
        {
            // Arrange
            var tournament = GetSomeMockTournaments().First(t => t.Participants.Count > 0);
            var player = tournament.Participants[0];
            var registrationDto = new RegistrationInTournamentDto { TournamentId = tournament.Id };
            var request = new RegisterPlayerInTournamentRequest(registrationDto);

            _mockTournamentRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>(),It.IsAny<string[]>())).ReturnsAsync(tournament);
            _mockCurrentUser.Setup(u => u.DomainUserId).Returns(player.Id);
            _mockPlayerRepository.Setup(r => r.GetByIdAsync(player.Id)).ReturnsAsync(player);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));
            Assert.Equal("Player is already registered for this tournament", exception.Message);
        }
        [Fact]
        public async Task Handle_NoAgeRestriction_AllowsRegistration()
        {
            // Arrange
            var tournament = GetSomeMockTournaments().First(t => t.MinimumAgeOfRegistration == 0);
            var player = new Player { Id = 1, Age = 10, Name = "test player", Email = "test@gmail.com"}; // Very young player, handler only handle age greateer than 0
            var registrationDto = new RegistrationInTournamentDto { TournamentId = tournament.Id };
            var request = new RegisterPlayerInTournamentRequest(registrationDto);

            _mockTournamentRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>(),It.IsAny<string[]>())).ReturnsAsync(tournament);
            _mockCurrentUser.Setup(u => u.DomainUserId).Returns(player.Id);
            _mockPlayerRepository.Setup(r => r.GetByIdAsync(player.Id)).ReturnsAsync(player);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mockTournamentRepository.Verify(r => r.SaveAsync(), Times.Once);
        }


        private static IEnumerable<Tournament> GetSomeMockTournaments()
        {
            var testAdmin = new Admin()
            {
                Id = 1,
                Name = "test admin",
                Email = "test@gmail.com",
                PhoneNumber = "12345"
            };

            var testGameType = new GameType()
            {
                Name = GameTypeSupported.TableTennis
            };

            yield return new Tournament
            {
                Id = 1,
                CreatedBy = testAdmin,
                Name = "Open Tournament",
                Status = TournamentStatus.RegistrationOpen,
                MaxParticipant = 10,
                MinimumAgeOfRegistration = 18,
                Participants = new List<Player>(),
                TournamentType = TournamentType.GroupStage,
                GameType = testGameType
            };

            yield return new Tournament
            {
                Id = 2,
                CreatedBy = testAdmin,
                Name = "Closed Tournament",
                Status = TournamentStatus.RegistrationClosed,
                MaxParticipant = 10,
                MinimumAgeOfRegistration = 18,
                Participants = new List<Player>(),
                GameType = testGameType,
                TournamentType = TournamentType.GroupStage

            };

            yield return new Tournament
            {
                Id = 3,
                CreatedBy = testAdmin,
                Name = "Registration deadline over tournament",
                Status = TournamentStatus.RegistrationOpen,
                RegistrationLastDate = DateTime.UtcNow.AddDays(-1),
                MaxParticipant = 2,
                MinimumAgeOfRegistration = 18,
                Participants = new List<Player>(),
                TournamentType = TournamentType.GroupStage,
                GameType = testGameType

            };
            yield return new Tournament
            {
                Id = 4,
                CreatedBy = testAdmin,
                Name = "Player already exists tournament",
                Status = TournamentStatus.RegistrationOpen,
                MaxParticipant = 2,
                MinimumAgeOfRegistration = 18,
                Participants = new List<Player>
                {
                     new Player { Id = 1, Name = "testplayer", Email = "testplayer@gmail.com", Age = 19 },
                },
                TournamentType = TournamentType.GroupStage,
                GameType = testGameType

            };
            yield return new Tournament
            {
                Id = 5,
                CreatedBy = testAdmin,
                Name = "Full Tournament",
                Status = TournamentStatus.RegistrationOpen,
                MaxParticipant = 2,
                MinimumAgeOfRegistration = 18,
                Participants = new List<Player>
                {
                     new Player { Id = 1, Name = "testplayer", Email = "testplayer@gmail.com" },
                     new Player { Id = 2, Name = "testplayer2", Email ="testemail@gmail.com" }
                },
                TournamentType = TournamentType.GroupStage,
                GameType = testGameType

            };

            yield return new Tournament
            {
                Id = 6,
                CreatedBy = testAdmin,
                Name = "No Age Limit Tournament",
                Status = TournamentStatus.RegistrationOpen,
                MaxParticipant = 10,
                MinimumAgeOfRegistration = 0,
                Participants = new List<Player>(),
                GameType = testGameType,
                TournamentType = TournamentType.GroupStage
            };
        }
    }
}