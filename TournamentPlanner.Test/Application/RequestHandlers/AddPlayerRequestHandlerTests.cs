using System;
using System.Collections.Generic;
using System.Linq;
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
    public class AddPlayerRequestHandlerTests
    {

        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IRepository<Player, Player>> _mockRepository;
        private readonly AddPlayerRequestHandler _handler;


        public AddPlayerRequestHandlerTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepository = new Mock<IRepository<Player, Player>>();
            _handler = new AddPlayerRequestHandler(_mockRepository.Object, _mockMapper.Object);

        }
        [Fact]
        public async Task Handle_ValidRequest_ReturnsPlayerDto()
        {
            // Arrange
            var addPlayerDto = new AddPlayerDto { Name = "Test Player", Email = "Test@gamil.com", Age = 23};
            var request = new AddPlayerRequest(addPlayerDto);
            var player = new Player { Id = 1, Name = "Test Player", Email = "Test@gmail.com", Age = 23};
            var expectedPlayerDto = new PlayerDto { Id = 1, Name = "Test Player", Age = 23};

            _mockMapper.Setup(m => m.Map<Player>(request.AddPlayerDto)).Returns(player);
            _mockMapper.Setup(m => m.Map<PlayerDto>(player)).Returns(expectedPlayerDto);
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Player>())).ReturnsAsync(player);
            _mockRepository.Setup(r => r.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedPlayerDto.Id, result.Id);
            Assert.Equal(expectedPlayerDto.Name, result.Name);
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<Player>()), Times.Once);
            _mockRepository.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_NullRequest_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Handle(null!, CancellationToken.None));
        }

    }
}