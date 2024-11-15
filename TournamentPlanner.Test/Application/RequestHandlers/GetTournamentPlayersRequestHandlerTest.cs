
using AutoMapper;
using Moq;
using TournamentPlanner.Application;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Exceptions;
using TournamentPlanner.Test.Fixtures;

namespace TournamentPlanner.Test.Application.RequestHandlers;



public class GetTournamentPlayersRequestHandlerTest
{
    private readonly Mock<IRepository<Tournament>> _tournamentRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly GetTournamentPlayersRequestHandler _handler;

    public GetTournamentPlayersRequestHandlerTest()
    {
        _tournamentRepository = new Mock<IRepository<Tournament>>();
        _mapper = new Mock<IMapper>();
        _handler = new GetTournamentPlayersRequestHandler(_tournamentRepository.Object, _mapper.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsPlayers()
    {
        // Arrange
        var request = new GetTournamentPlayersRequest(1);
        var tournament = TournamentFixtures.GetPopulatedGroupTournamentFresh();
        _tournamentRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<string[]>())).ReturnsAsync(tournament);
        _mapper.Setup(m => m.Map<IEnumerable<PlayerDto>>(It.IsAny<List<Player>>())).Returns(tournament.Participants.Select(p => new PlayerDto { Id = p.Id, Name = p.Name}));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(tournament.Participants.Count, result.Count());
    }

    [Fact]
    public async Task Handle_InvalidRequest_ThrowsException()
    {
        // Arrange
        var request = new GetTournamentPlayersRequest(1);
        _tournamentRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<string[]>())).ReturnsAsync((Tournament)null!);

        // Act and Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_NullRequest_ThrowsException()
    {
        // Act and Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Handle(null!, CancellationToken.None));
    }

}
