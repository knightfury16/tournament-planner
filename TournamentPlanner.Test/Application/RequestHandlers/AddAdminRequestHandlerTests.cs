using AutoMapper;
using Moq;
using TournamentPlanner.Application;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.Test.Application.RequestHandlers
{
    public class AddAdminRequestHandlerTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IRepository<Admin, Admin>> _mockRepository;
        private readonly AddAdminRequestHandler _handler;




        public AddAdminRequestHandlerTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepository = new Mock<IRepository<Admin, Admin>>();
            _handler = new AddAdminRequestHandler(_mockMapper.Object, _mockRepository.Object);

        }
        [Fact]
        public async Task Handle_ValidRequest_ReturnsAdminDto()
        {
            // Arrange
            var addAdminDto = new AddAdminDto { Name = "Test Admin", Email = "Test@gamil.com", PhoneNumber = "12345" };
            var request = new AddAdminRequest(addAdminDto);
            var admin = new Admin { Id = 1, Name = "Test Admin", Email = "Test@gmail.com", PhoneNumber = "12345" };
            var expectedAdminDto = new AdminDto { Id = 1, Name = "Test Admin", Email = "Test@gmail.com", PhoneNumber = "12345" };

            _mockMapper.Setup(m => m.Map<Admin>(request.AddAdminDto)).Returns(admin);
            _mockMapper.Setup(m => m.Map<AdminDto>(admin)).Returns(expectedAdminDto);
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Admin>())).ReturnsAsync(admin);
            _mockRepository.Setup(r => r.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedAdminDto.Id, result.Id);
            Assert.Equal(expectedAdminDto.Name, result.Name);
            Assert.Equal(expectedAdminDto.PhoneNumber, result.PhoneNumber);
            Assert.Equal(expectedAdminDto.Email, result.Email);
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<Admin>()), Times.Once);
            _mockRepository.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_NullRequest_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Handle(null, CancellationToken.None));
        }

    }
}