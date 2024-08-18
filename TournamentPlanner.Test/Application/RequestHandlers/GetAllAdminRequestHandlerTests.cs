using AutoMapper;
using Moq;
using TournamentPlanner.Application;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.Test.Application.RequestHandlers
{
    public class GetAllAdminRequestHandlerTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IRepository<Admin, Admin>> _mockRepository;
        private readonly GetAllAdminRequestHandler _handler;

        public GetAllAdminRequestHandlerTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepository = new Mock<IRepository<Admin, Admin>>();
            _handler = new GetAllAdminRequestHandler(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_NoNameFilter_ReturnsAllAdmins()
        {
            // Arrange
            var request = new GetAllAdminRequest();
            var admins = new List<Admin>
            {
                new Admin { Id = 1, Name = "Admin 1", Email = "Test1@gmail.com", PhoneNumber = "12345" },
                new Admin { Id = 2, Name = "Admin 2" , Email = "Test2@gmail.com", PhoneNumber = "12345" }
            };
            var expectedAdminDtos = new List<AdminDto>
            {
                new AdminDto { Id = 1, Name = "Admin 1", Email = "Test1@gmail.com", PhoneNumber = "12345"  },
                new AdminDto { Id = 2, Name = "Admin 2", Email = "Test2@gmail.com", PhoneNumber = "12345"  }
            };

            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(admins);
            _mockMapper.Setup(m => m.Map<IEnumerable<AdminDto>>(admins)).Returns(expectedAdminDtos);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(expectedAdminDtos, result);
        }

        [Fact]
        public async Task Handle_WithNameFilter_ReturnsFilteredAdmins()
        {
            // Arrange
            var request = new GetAllAdminRequest { Name = "admin 1" };
            var admins = new List<Admin>
            {
                new Admin { Id = 1, Name = "Admin 1" , Email = "Test1@gmail.com", PhoneNumber = "12345" }
            };
            var expectedAdminDtos = new List<AdminDto>
            {
                new AdminDto { Id = 1, Name = "Admin 1" , Email = "Test1@gmail.com", PhoneNumber = "12345" }
            };

            _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<Func<Admin, bool>>()))
                .ReturnsAsync(admins);
            _mockMapper.Setup(m => m.Map<IEnumerable<AdminDto>>(admins)).Returns(expectedAdminDtos);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(expectedAdminDtos, result);
        }

        [Fact]
        public async Task Handle_NoAdminsFound_ReturnsEmptyList()
        {
            // Arrange
            var request = new GetAllAdminRequest();
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Admin>());
            _mockMapper.Setup(m => m.Map<IEnumerable<AdminDto>>(It.IsAny<IEnumerable<Admin>>()))
                .Returns(new List<AdminDto>());

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}