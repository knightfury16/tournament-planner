using AutoMapper;
using Moq;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Application.Request;
using TournamentPlanner.Application.RequestHandler;
using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.Test.Application.RequestHandlers
{
    public class GetAllPlayerRequestHandlerTests
    {

        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IRepository<Player, Player>> _mockRepository;
        private readonly GetAllPlayerRequestHandler _handler;

        public GetAllPlayerRequestHandlerTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepository = new Mock<IRepository<Player, Player>>();
            _handler = new GetAllPlayerRequestHandler(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_NoNameFilter_ReturnsAllPlayers()
        {
            // Arrange
            var request = new GetAllPlayerRequest();
            var players = new List<Player>
            {
                new Player { Id = 1, Name = "Player 1", Email = "Test1@gmail.com", Age = 23, Weight = 66 },
                new Player { Id = 2, Name = "Player 2" , Email = "Test2@gmail.com", Age = 25, Weight = 70}
            };
            var expectedplayerDtos = new List<PlayerDto>
            {
                new PlayerDto { Id = 1, Name = "Player 1", Age = 23},
                new PlayerDto { Id = 2, Name = "Player 2", Age = 25}
            };

            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(players);
            _mockMapper.Setup(m => m.Map<IEnumerable<PlayerDto>>(players)).Returns(expectedplayerDtos);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(expectedplayerDtos, result);
        }

        [Fact]
        public async Task Handle_WithNameFilter_ReturnsFilteredPlayers()
        {
            // Arrange
            var request = new GetAllPlayerRequest { Name = "player 1" };
            var players = new List<Player>
            {
                new Player { Id = 1, Name = "Player 1" , Email = "Test1@gmail.com", Age = 22}
            };
            var expectedPlayerDtos = new List<PlayerDto>
            {
                new PlayerDto { Id = 1, Name = "Player 1", Age = 22}
            };


            _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<Func<Player, bool>>()))
                .ReturnsAsync(players);
            _mockMapper.Setup(m => m.Map<IEnumerable<PlayerDto>>(players)).Returns(expectedPlayerDtos);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(expectedPlayerDtos, result);
        }

        [Fact]
        public async Task Handle_NoPlayerFound_ReturnsEmptyList()
        {
            // Arrange
            var request = new GetAllPlayerRequest();
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Player>());
            _mockMapper.Setup(m => m.Map<IEnumerable<PlayerDto>>(It.IsAny<IEnumerable<Player>>()))
                .Returns(new List<PlayerDto>());

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}