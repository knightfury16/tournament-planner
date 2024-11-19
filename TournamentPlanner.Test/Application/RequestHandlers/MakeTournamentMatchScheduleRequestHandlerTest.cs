using AutoMapper;
using Moq;
using TournamentPlanner.Application;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Application.Services;
using TournamentPlanner.Domain.Entities;
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
        _tournamentServiceMock.Setup(s => s.CanISchedule(It.IsAny<Tournament>())).ReturnsAsync(true);
        _matchServiceMock.Setup(m => m.CreateMatches(It.IsAny<Tournament>(), It.IsAny<SchedulingInfo>())).ReturnsAsync(matches);

        _matchSchedulerMock.Setup(ms => ms.DefaultMatchScheduler(It.IsAny<List<Match>>(), It.IsAny<SchedulingInfo>())).Returns(matches);
        _mapper.Setup(m => m.Map<IEnumerable<MatchDto>>(It.IsAny<List<Match>>())).Returns(matchDtos);

        var request = new MakeTournamentMatchScheduleRequest
        (
            tournament.Id,
            new SchedulingInfo
            {
                StartDate = DateTime.UtcNow,
                EachMatchTime = TimeSpan.FromMinutes(20)
            }
        );

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count()); 
    }

    // [Fact]
    // public async Task Handle_GivenTournamentAlreadyCompleted_ThrowsBadRequestException()
    // {
    //     // Arrange
    //     var tournament = TournamentFixtures.GetTournament();
    //     tournament.Status = TournamentStatus.Completed;
    //     _tournamentRepository.GetByIdAsync(tournament.Id, Arg.Any<string[]>()).Returns(Task.FromResult(tournament as Tournament));

    //     var request = new MakeTournamentMatchScheduleRequest
    //     {
    //         TournamentId = tournament.Id,
    //         SchedulingInfo = new SchedulingInfo
    //         {
    //             StartDate = DateTime.UtcNow,
    //             EndDate = DateTime.UtcNow.AddDays(1),
    //             MatchDuration = TimeSpan.FromHours(1)
    //         }
    //     };

    //     // Act and Assert
    //     await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));
    // }

    // [Fact]
    // public async Task Handle_GivenDrawsNotMadeOrPreviousDrawsNotComplete_ThrowsBadRequestException()
    // {
    //     // Arrange
    //     var tournament = TournamentFixtures.GetTournament();
    //     _tournamentRepository.GetByIdAsync(tournament.Id, Arg.Any<string[]>()).Returns(Task.FromResult(tournament as Tournament));
    //     _tournamentService.CanISchedule(Arg.Any<Tournament>()).Returns(Task.FromResult(false as bool));

    //     var request = new MakeTournamentMatchScheduleRequest
    //     {
    //         TournamentId = tournament.Id,
    //         SchedulingInfo = new SchedulingInfo
    //         {
    //             StartDate = DateTime.UtcNow,
    //             EndDate = DateTime.UtcNow.AddDays(1),
    //             MatchDuration = TimeSpan.FromHours(1)
    //         }
    //     };

    //     // Act and Assert
    //     await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));
    // }
}
