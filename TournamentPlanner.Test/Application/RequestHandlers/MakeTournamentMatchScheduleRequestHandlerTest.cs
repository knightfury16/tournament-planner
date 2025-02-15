using AutoMapper;
using Moq;
using TournamentPlanner.Application;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Application.Services;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Exceptions;
using TournamentPlanner.Test.Fixtures;
using Match = TournamentPlanner.Domain.Entities.Match;

namespace TournamentPlanner.Test.Application.RequestHandlers;

public class MakeTournamentMatchScheduleRequestHandlerTest
{
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IRepository<Tournament>> _tournamentRepository;
    private readonly Mock<IDrawService> _drawServiceMock;
    private readonly Mock<IMatchService> _matchServiceMock;
    private readonly Mock<ITournamentService> _tournamentServiceMock;
    private readonly Mock<IMatchScheduler> _matchSchedulerMock;
    private readonly MakeTournamentMatchScheduleRequestHandler _handler;

    public MakeTournamentMatchScheduleRequestHandlerTest()
    {
        _mapper = new Mock<IMapper>();
        _tournamentRepository = new Mock<IRepository<Tournament>>();
        _drawServiceMock = new Mock<IDrawService>();
        _matchServiceMock = new Mock<IMatchService>();
        _tournamentServiceMock = new Mock<ITournamentService>();
        _matchSchedulerMock = new Mock<IMatchScheduler>();
        _handler = new MakeTournamentMatchScheduleRequestHandler(_tournamentRepository.Object, _mapper.Object, _drawServiceMock.Object, _matchServiceMock.Object, _tournamentServiceMock.Object, _matchSchedulerMock.Object);
    }


    [Fact]
    public async Task Handle_GivenValidRequest_ReturnsScheduledMatches()
    {
        // Arrange
        var tournament = TournamentFixtures.GetPopulatedGroupTournamentFresh();
        var firstMatch = MatchFixtures.GetSinglemMatchOfGroup();
        var secondMatch = MatchFixtures.GetSinglemMatchOfGroup();
        var matches = new List<Match> { firstMatch, secondMatch };
        var matchDtos = matches.Select(m => new MatchDto { FirstPlayer = new PlayerDto { Name = "Test Player1" }, SecondPlayer = new PlayerDto { Name = "Test Player2" } }).ToList();
        _tournamentRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<string[]>())).ReturnsAsync(tournament);
        _tournamentServiceMock.Setup(s => s.AmITheCreator(tournament)).Returns(true);
        _tournamentServiceMock.Setup(s => s.CanISchedule(It.IsAny<Tournament>())).ReturnsAsync(true);
        _matchServiceMock.Setup(m => m.CreateMatches(It.IsAny<Tournament>(), It.IsAny<SchedulingInfo>())).ReturnsAsync(matches);

        _matchSchedulerMock.Setup(ms => ms.DefaultMatchScheduler(It.IsAny<List<Match>>(), It.IsAny<SchedulingInfo>())).ReturnsAsync(matches);
        _mapper.Setup(m => m.Map<IEnumerable<MatchDto>>(It.IsAny<List<Match>>())).Returns(matchDtos);

        var request = new MakeTournamentMatchScheduleRequest
        (
            tournament.Id,
            new SchedulingInfo
            {
                StartDate = DateTime.UtcNow,
                EachMatchTime = TimeSpan.FromMinutes(20).ToString(),
            }
        );

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task Handle_TournamentIsNull_ThrowsException()
    {
        // Arrange
        var request = new MakeTournamentMatchScheduleRequest
        (
            1,
            new SchedulingInfo
            {
                StartDate = DateTime.UtcNow,
                EachMatchTime = TimeSpan.FromMinutes(20).ToString()
            }
        );

        _tournamentRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<string[]>())).ReturnsAsync((Tournament)null!);

        // Act and Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));
    }
}
