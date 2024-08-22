using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Application.Request;
using TournamentPlanner.Application.RequestHandler;
using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.Test.Application.RequestHandlers
{
    public class GetPlayerByIdRequestHandlerTests

    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IRepository<Player>> _mockRepository;
        private readonly GetPlayerByIdRequestHandler _handler;

        public GetPlayerByIdRequestHandlerTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepository = new Mock<IRepository<Player>>();
            _handler = new GetPlayerByIdRequestHandler(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ExistingId_ReturnsPlayerDto()
        {
            // Arrange
            var playerId = 1;
            var request = new GetPlayerByIdRequest(playerId);
            var player = new Player { Id = playerId, Name = "Test Player", Email = "Test@gmail.com", Age = 23, Weight = 78 };
            var expectedPlayerDto = new FullPlayerDto { Id = playerId, Name = "Test Player", Weight = 78, Age = 23 };

            _mockRepository.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Player, bool>>>(),
                It.IsAny<string[]>()))
                .ReturnsAsync(new List<Player> { player });
            _mockMapper.Setup(m => m.Map<FullPlayerDto>(player)).Returns(expectedPlayerDto);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedPlayerDto.Id, result.Id);
            Assert.Equal(expectedPlayerDto.Name, result.Name);
        }

        [Fact]
        public async Task Handle_NonExistingId_ReturnsNull()
        {
            // Arrange
            var playerId = 999;
            var request = new GetPlayerByIdRequest(playerId);

            _mockRepository.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Player, bool>>>(),
                It.IsAny<string[]>()))
                .ReturnsAsync(new List<Player>());

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }
    }
}