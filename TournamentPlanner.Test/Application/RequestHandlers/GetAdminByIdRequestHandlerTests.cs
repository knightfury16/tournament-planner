using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using TournamentPlanner.Application;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.Test.Application.RequestHandlers
{
    public class GetAdminByIdRequestHandlerTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IRepository<Admin, Admin>> _mockRepository;
        private readonly GetAdminByIdRequestHandler _handler;

        public GetAdminByIdRequestHandlerTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepository = new Mock<IRepository<Admin, Admin>>();
            _handler = new GetAdminByIdRequestHandler(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ExistingId_ReturnsAdminDto()
        {
            // Arrange
            var adminId = 1;
            var request = new GetAdminByIdRequest(adminId);
            var admin = new Admin { Id = adminId, Name = "Test Admin", Email = "Test@gmail.com", PhoneNumber = "12345" };
            var expectedAdminDto = new AdminDto { Id = adminId, Name = "Test Admin", Email = "Test@gmail.com", PhoneNumber = "12345" };

            _mockRepository.Setup(r => r.GetAllAsync(
                It.IsAny<Func<Admin, bool>>(),
                It.IsAny<string[]>()))
                .ReturnsAsync(new List<Admin> { admin });
            _mockMapper.Setup(m => m.Map<AdminDto>(admin)).Returns(expectedAdminDto);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedAdminDto.Id, result.Id);
            Assert.Equal(expectedAdminDto.Name, result.Name);
        }

        [Fact]
        public async Task Handle_NonExistingId_ReturnsNull()
        {
            // Arrange
            var adminId = 999;
            var request = new GetAdminByIdRequest(adminId);

            _mockRepository.Setup(r => r.GetAllAsync(
                It.IsAny<Func<Admin, bool>>(),
                It.IsAny<string[]>()))
                .ReturnsAsync(new List<Admin>());

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }
    }
}