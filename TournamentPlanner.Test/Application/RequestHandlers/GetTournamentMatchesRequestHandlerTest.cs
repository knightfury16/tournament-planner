namespace TournamentPlanner.Test.Application.RequestHandlers;


using Xunit;
using Moq;
using AutoMapper;
using TournamentPlanner.Application;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Exceptions;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using TournamentPlanner.Test.Fixtures;

public class GetTournamentMatchesRequestHandlerTest
{
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IRepository<Tournament>> _tournamentRepository;
    private readonly GetTournamentMatchesRequestHandler _handler;

    public GetTournamentMatchesRequestHandlerTest()
    {
        _mapper = new Mock<IMapper>();
        _tournamentRepository = new Mock<IRepository<Tournament>>();
        _handler = new GetTournamentMatchesRequestHandler(_tournamentRepository.Object, _mapper.Object);
    }

    [Fact]
    public async Task Handle_GivenValidRequest_ReturnsTournamentMatches()
    {
        // Arrange
        var tournament = TournamentFixtures.GetTournament();
        var match = MatchFixtures.GetSinglemMatchOfGroup();
        var expectedMatches = new List<MatchDto> { new MatchDto { FirstPlayer = match.FirstPlayer.ToPlayerDto(), SecondPlayer = match.SecondPlayer.ToPlayerDto() } };

        _tournamentRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<string[]>())).ReturnsAsync(tournament);
        _mapper.Setup(m => m.Map<IEnumerable<MatchDto>>(tournament.Matches)).Returns(expectedMatches);

        var request = new GetTournamentMatchesRequest(tournament.Id);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(expectedMatches, result);
    }

    [Fact]
    public async Task Handle_TournamentIsNull_ThrowsException()
    {
        // Arrange
        var request = new GetTournamentMatchesRequest(1);

        _tournamentRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<string[]>())).ReturnsAsync((Tournament)null!);

        // Act and Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));
    }
}

