using Moq;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Application.UseCases.PlayerUseCase;
using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.Test.Application
{
    public class PlayerUseCaseTest
    {
        private readonly PlayerDto playerDto;
        private readonly Player player;
        private readonly Mock<IRepository<Player, Player>> playerRepositoryMock;
        private readonly PlayerUseCase playerUseCase;

        public PlayerUseCaseTest()
        { // Arrange common setup
            playerDto = new PlayerDto
            {
                Name = "John Doe",
                PhoneNumber = "1234567890",
                Email = "john.doe@example.com",
                TournamentId = 1
            };

            player = new Player
            {
                Name = playerDto.Name,
                PhoneNumber = playerDto.PhoneNumber,
                Email = playerDto.Email,
                TournamentId = playerDto.TournamentId
            };

            playerRepositoryMock = new Mock<IRepository<Player, Player>>();
            playerUseCase = new PlayerUseCase(playerRepositoryMock.Object);

        }


        [Fact]
        public async Task AddPlayerAsync_ShouldAddPlayerSuccessfully_WhenValidPlayerDtoIsProvided()
        {
            playerRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Player>())).Returns(Task.FromResult(player));
            playerRepositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            var playerUseCase = new PlayerUseCase(playerRepositoryMock.Object);

            // Act
            var result = await playerUseCase.AddPlayerAsync(playerDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(playerDto.Name, result.Name);
            Assert.Equal(playerDto.PhoneNumber, result.PhoneNumber);
            Assert.Equal(playerDto.Email, result.Email);
            Assert.Equal(playerDto.TournamentId, result.TournamentId);

            playerRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Player>()), Times.Once);
            playerRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task AddPlayerAsync_ShouldHandleNullPlayerDtoGracefully()
        {
            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => playerUseCase.AddPlayerAsync(null));

            playerRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Player>()), Times.Never);
            playerRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task GetPlayersAsync_ReturnsAllPlayers_WhenPlayerNameIsNull()
        {
            // Arrange
            var players = new List<Player>
            {
                new Player { Name = "Player1", PhoneNumber = "1234567890", Email = "player1@example.com", TournamentId = 1 },
                new Player { Name = "Player2", PhoneNumber = "0987654321", Email = "player2@example.com", TournamentId = 2 }
            };

            playerRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(players);

            // Act
            var result = await playerUseCase.GetPlayersAsync(null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetPlayersAsync_HandlesEmptyStringAsPlayerName()
        {
            // Arrange
            var players = new List<Player>
            {
                new Player { Name = "Player1", PhoneNumber = "1234567890", Email = "player1@example.com", TournamentId = 1 },
                new Player { Name = "Player2", PhoneNumber = "0987654321", Email = "player2@example.com", TournamentId = 2 }
            };
            playerRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(players);

            // Act
            var result = await playerUseCase.GetPlayersAsync(string.Empty);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetPlayersAsync_ShouldReturnPlayersMatchingName()
        {
            // Arrange
            var playerName = "Player1";
            var players = new List<Player>
            {
                new Player { Name = playerName, PhoneNumber = "1234567890", Email = "player1@example.com", TournamentId = 1 },
                new Player { Name = "Player2", PhoneNumber = "0987654321", Email = "player2@example.com", TournamentId = 2 }
            };
            var matchingPlayers = players.Where(p => p.Name == playerName).ToList();

            playerRepositoryMock.Setup(repo => repo.GetByNameAsync(playerName)).ReturnsAsync(matchingPlayers);

            // Act
            var result = await playerUseCase.GetPlayersAsync(playerName);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result); // Ensure only one player is returned
            Assert.Equal(playerName, result.First().Name); // Ensure the returned player's name matches the expected name
            playerRepositoryMock.Verify(repo => repo.GetByNameAsync(playerName), Times.Once);
        }

        [Fact]
        public async Task GetPlayerById_RetrievesPlayer_WhenIdIsValid()
        {
            // Arrange
            var playerId = 1;
            var expectedPlayer = new Player { Id = playerId, Name = "John Doe" };
            playerRepositoryMock.Setup(repo => repo.GetByIdAsync(playerId)).ReturnsAsync(expectedPlayer);

            // Act
            var result = await playerUseCase.GetPlayerById(playerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedPlayer.Id, result.Id);
            Assert.Equal(expectedPlayer.Name, result.Name);
        }

        [Fact]
        public async Task GetPlayerById_ThrowsException_WhenPlayerIdDoesNotExist()
        {
            // Arrange
            var playerId = 1;
            playerRepositoryMock.Setup(repo => repo.GetByIdAsync(playerId)).ReturnsAsync((Player)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => playerUseCase.GetPlayerById(playerId));
            Assert.Equal("Player with the speficied Id not found!", exception.Message);
        }

    }
}