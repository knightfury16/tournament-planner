using System.Linq.Expressions;
using AutoMapper;
using Moq;
using TournamentPlanner.Application;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Application.Enums;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Enum;

namespace TournamentPlanner.Tests.Application
{
    public class GetTournamentRequestHandlerTests
    {
        private readonly Mock<IRepository<Tournament>> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetTournamentRequestHandler _handler;

        public GetTournamentRequestHandlerTests()
        {
            _mockRepository = new Mock<IRepository<Tournament>>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetTournamentRequestHandler(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_WithNoFilters_ReturnsAllTournaments()
        {
            // Arrange
            var request = new GetTournamentRequest { SearchCategory = TournamentSearchCategory.All };
            var tournaments = GetSampleTournaments();
            _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<IEnumerable<Expression<Func<Tournament, bool>>>>(), It.IsAny<string[]>()))
                .ReturnsAsync(tournaments);
            _mockMapper.Setup(m => m.Map<IEnumerable<TournamentDto>>(It.IsAny<IEnumerable<Tournament>>()))
                .Returns((IEnumerable<Tournament> src) => src.Select(t => new TournamentDto { Id = t.Id, Name = t.Name }));

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(tournaments.Count, result.Count());
        }

        [Fact]
        public async Task Handle_WithNameFilter_ReturnsFilteredTournaments()
        {
            // Arrange
            var request = new GetTournamentRequest { Name = "test", SearchCategory = TournamentSearchCategory.All };
            var tournaments = GetSampleTournaments().Where(t => t.Name.ToLower().Contains("test")).ToList();

            _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<IEnumerable<Expression<Func<Tournament, bool>>>>(), It.IsAny<string[]>()))
                .ReturnsAsync(tournaments);
            _mockMapper.Setup(m => m.Map<IEnumerable<TournamentDto>>(It.IsAny<IEnumerable<Tournament>>()))
                .Returns((IEnumerable<Tournament> src) => src.Select(t => new TournamentDto { Id = t.Id, Name = t.Name }));

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Count(), tournaments.Count);
            Assert.All(result, t => Assert.Contains("test", t.Name.ToLower()));
        }
        [Fact]
        public void Handle_GetTournamentRequest_ConvertNameToLower()
        {
            // Arrange
            var testName = "TesT";
            var request = new GetTournamentRequest { Name = testName, SearchCategory = TournamentSearchCategory.All };

            // Act

            // Assert
            Assert.Equal(testName.ToLower(), request.Name);
        }


        [Fact]
        public async Task Handle_WithStatusFilter_ReturnsFilteredTournaments()
        {
            // Arrange
            var request = new GetTournamentRequest { Status = TournamentStatus.Ongoing, SearchCategory = TournamentSearchCategory.All };
            var tournaments = GetSampleTournaments().Where(t => t.Status == TournamentStatus.Ongoing).ToList();
            _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<IEnumerable<Expression<Func<Tournament, bool>>>>(), It.IsAny<string[]>()))
                .ReturnsAsync(tournaments);
            _mockMapper.Setup(m => m.Map<IEnumerable<TournamentDto>>(It.IsAny<IEnumerable<Tournament>>()))
                .Returns((IEnumerable<Tournament> src) => src.Select(t => new TournamentDto { Id = t.Id, Name = t.Name, Status = t.Status.ToString() }));

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.All(result, t => Assert.Equal(TournamentStatus.Ongoing.ToString(), t.Status));
        }

        [Fact]
        public async Task Handle_WithGameTypeFilter_ReturnsFilteredTournaments()
        {
            // Arrange
            var request = new GetTournamentRequest { GameTypeSupported = GameTypeSupported.Chess, SearchCategory = TournamentSearchCategory.All };
            var tournaments = GetSampleTournaments().Where(t => t.GameType.Name == GameTypeSupported.Chess).ToList();
            _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<IEnumerable<Expression<Func<Tournament, bool>>>>(), It.IsAny<string[]>()))
                .ReturnsAsync(tournaments);

            _mockMapper.Setup(m => m.Map<GameTypeDto>(It.IsAny<GameType>()))
             .Returns((GameType g) => new GameTypeDto
             {
                 Name = g.Name.ToString()
             });

            _mockMapper.Setup(m => m.Map<IEnumerable<TournamentDto>>(It.IsAny<IEnumerable<Tournament>>()))
                .Returns((IEnumerable<Tournament> src) => src.Select(t => new TournamentDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    GameTypeDto = _mockMapper.Object.Map<GameTypeDto>(t.GameType)
                }));

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.All(result, t => Assert.Equal(GameTypeSupported.Chess.ToString(), t.GameTypeDto?.Name));
        }

        [Fact]
        public async Task Handle_WithDateRangeFilter_ReturnsFilteredTournaments()
        {
            // Arrange
            var startDate = DateTime.UtcNow.Date;
            var endDate = startDate.AddDays(7);
            var request = new GetTournamentRequest { StartDate = startDate, EndDate = endDate, SearchCategory = TournamentSearchCategory.All };
            var tournaments = GetSampleTournaments().Where(t => t.StartDate >= startDate && t.EndDate <= endDate).ToList();
            _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<IEnumerable<Expression<Func<Tournament, bool>>>>(), It.IsAny<string[]>()))
                .ReturnsAsync(tournaments);
            _mockMapper.Setup(m => m.Map<IEnumerable<TournamentDto>>(It.IsAny<IEnumerable<Tournament>>()))
                .Returns((IEnumerable<Tournament> src) => src.Select(t => new TournamentDto { Id = t.Id, Name = t.Name, StartDate = t.StartDate, EndDate = t.EndDate }));

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.All(result, t => Assert.True(t.StartDate >= startDate && t.EndDate <= endDate));
        }

        [Fact]
        public async Task Handle_WithSearchCategoryRecent_ReturnsRecentTournaments()
        {
            // Arrange
            var request = new GetTournamentRequest { SearchCategory = TournamentSearchCategory.Recent };
            var today = DateTime.UtcNow.Date;
            var tournaments = GetSampleTournaments().Where(t => t.EndDate <= today && t.EndDate >= today.AddDays(-7)).ToList();
            _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<IEnumerable<Expression<Func<Tournament, bool>>>>(), It.IsAny<string[]>()))
                .ReturnsAsync(tournaments);
            _mockMapper.Setup(m => m.Map<IEnumerable<TournamentDto>>(It.IsAny<IEnumerable<Tournament>>()))
                .Returns((IEnumerable<Tournament> src) => src.Select(t => new TournamentDto { Id = t.Id, Name = t.Name, EndDate = t.EndDate }));

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.All(result, t => Assert.True(t.EndDate <= today && t.EndDate >= today.AddDays(-7)));
        }

        [Fact]
        public async Task Handle_WithSearchCategoryThisWeek_ReturnsThisWeekTournaments()
        {
            // Arrange
            var request = new GetTournamentRequest { SearchCategory = TournamentSearchCategory.ThisWeek };
            var today = DateTime.UtcNow.Date;
            var weekStart = today.AddDays(-(int)today.DayOfWeek);
            var weekEnd = weekStart.AddDays(7);
            var tournaments = GetSampleTournaments().Where(t => t.StartDate >= weekStart && t.StartDate < weekEnd).ToList();
            _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<IEnumerable<Expression<Func<Tournament, bool>>>>(), It.IsAny<string[]>()))
                .ReturnsAsync(tournaments);
            _mockMapper.Setup(m => m.Map<IEnumerable<TournamentDto>>(It.IsAny<IEnumerable<Tournament>>()))
                .Returns((IEnumerable<Tournament> src) => src.Select(t => new TournamentDto { Id = t.Id, Name = t.Name, StartDate = t.StartDate }));

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.All(result, t => Assert.True(t.StartDate >= weekStart && t.StartDate < weekEnd));
        }

        [Fact]
        public async Task Handle_WithSearchCategoryUpcoming_ReturnsUpcomingTournaments()
        {
            // Arrange
            var request = new GetTournamentRequest { SearchCategory = TournamentSearchCategory.Upcoming };
            var today = DateTime.UtcNow.Date;
            var tournaments = GetSampleTournaments().Where(t => t.StartDate > today).ToList();
            _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<IEnumerable<Expression<Func<Tournament, bool>>>>(), It.IsAny<string[]>()))
                .ReturnsAsync(tournaments);
            _mockMapper.Setup(m => m.Map<IEnumerable<TournamentDto>>(It.IsAny<IEnumerable<Tournament>>()))
                .Returns((IEnumerable<Tournament> src) => src.Select(t => new TournamentDto { Id = t.Id, Name = t.Name, StartDate = t.StartDate }));

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.All(result, t => Assert.True(t.StartDate > today));
        }

        [Fact]
        public async Task Handle_WithSearchCategoryAll_ReturnsAllTournaments()
        {
            // Arrange
            var request = new GetTournamentRequest { SearchCategory = TournamentSearchCategory.All };
            var tournaments = GetSampleTournaments();
            _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<IEnumerable<Expression<Func<Tournament, bool>>>>(), It.IsAny<string[]>()))
                .ReturnsAsync(tournaments);
            _mockMapper.Setup(m => m.Map<IEnumerable<TournamentDto>>(It.IsAny<IEnumerable<Tournament>>()))
                .Returns((IEnumerable<Tournament> src) => src.Select(t => new TournamentDto { Id = t.Id, Name = t.Name }));

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(tournaments.Count, result.Count());
        }

        [Fact]
        public async Task Handle_WithMultipleFilters_ReturnsCorrectlyFilteredTournaments()
        {
            // Arrange
            var request = new GetTournamentRequest
            {
                Name = "test",
                Status = TournamentStatus.Ongoing,
                GameTypeSupported = GameTypeSupported.Chess,
                StartDate = DateTime.UtcNow.Date,
                EndDate = DateTime.UtcNow.Date.AddDays(7),
                SearchCategory = TournamentSearchCategory.All
            };
            var tournaments = GetSampleTournaments().Where(t =>
                t.Name.ToLower().Contains("test") &&
                t.Status == TournamentStatus.Ongoing &&
                t.GameType.Name == GameTypeSupported.Chess &&
                t.StartDate >= request.StartDate &&
                t.EndDate <= request.EndDate
            ).ToList();
            _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<IEnumerable<Expression<Func<Tournament, bool>>>>(), It.IsAny<string[]>()))
                .ReturnsAsync(tournaments);

            _mockMapper.Setup(m => m.Map<GameTypeDto>(It.IsAny<GameType>()))
             .Returns((GameType g) => new GameTypeDto
             {
                 Name = g.Name.ToString()
             });
             
            _mockMapper.Setup(m => m.Map<IEnumerable<TournamentDto>>(It.IsAny<IEnumerable<Tournament>>()))
                .Returns((IEnumerable<Tournament> src) => src.Select(t => new TournamentDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Status = t.Status.ToString(),
                    GameTypeDto = _mockMapper.Object.Map<GameTypeDto>(t.GameType), // map GameType to GameTypeDto 
                    StartDate = t.StartDate,
                    EndDate = t.EndDate
                }));

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.All(result, t =>
            {
                Assert.Contains("test", t.Name.ToLower());
                Assert.Equal(TournamentStatus.Ongoing.ToString(), t.Status);
                Assert.Equal(GameTypeSupported.Chess.ToString(), t.GameTypeDto?.Name);
                Assert.True(t.StartDate >= request.StartDate);
                Assert.True(t.EndDate <= request.EndDate);
            });
        }

        private List<Tournament> GetSampleTournaments()
        {
            Admin testAdmin = new Admin()
            {
                Name = "Test creator",
                Email = "test@gmail.com",
                PhoneNumber = "12345"
            };
            return new List<Tournament>
            {
                new Tournament { Id = 1, Name = "Test Tournament 1", CreatedBy = testAdmin, Status = TournamentStatus.Ongoing, GameType = new GameType { Name = GameTypeSupported.Chess }, StartDate = DateTime.UtcNow.AddDays(2), EndDate = DateTime.UtcNow.AddDays(3), TournamentType = TournamentType.GroupStage },
                new Tournament { Id = 2, Name = "Another Tournament", CreatedBy = testAdmin,Status = TournamentStatus.Draft, GameType = new GameType { Name = GameTypeSupported.TableTennis }, StartDate = DateTime.UtcNow.AddDays(-1), EndDate = DateTime.UtcNow.AddDays(2), TournamentType = TournamentType.GroupStage },
                new Tournament { Id = 3, Name = "Test Tournament 2", CreatedBy = testAdmin,Status = TournamentStatus.Completed, GameType = new GameType { Name = GameTypeSupported.Chess }, StartDate = DateTime.UtcNow.AddDays(-5), EndDate = DateTime.UtcNow.AddDays(-2), TournamentType = TournamentType.GroupStage },
                new Tournament { Id = 4, Name = "Upcoming Test",CreatedBy = testAdmin, Status = TournamentStatus.Ongoing, GameType = new GameType { Name = GameTypeSupported.TableTennis }, StartDate = DateTime.UtcNow.AddDays(5), EndDate = DateTime.UtcNow.AddDays(10), TournamentType = TournamentType.GroupStage }
            };
        }
    }
}