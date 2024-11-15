using AutoMapper;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Exceptions;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class GetTournamentPlayersRequestHandler : IRequestHandler<GetTournamentPlayersRequest, IEnumerable<PlayerDto>>
{
    private readonly IRepository<Tournament> _tournamentRepository;
    private readonly IMapper _mapper;

    public GetTournamentPlayersRequestHandler(IRepository<Tournament> tournamentRepository, IMapper mapper)
    {
        _tournamentRepository = tournamentRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PlayerDto>?> Handle(GetTournamentPlayersRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var tournament = await _tournamentRepository.GetByIdAsync(request.TournamentId, [nameof(Tournament.Participants)]);

        if(tournament == null)throw new NotFoundException(nameof(tournament));
        if(tournament.Participants == null)throw new NullReferenceException(nameof(tournament.Participants));

        var players = tournament.Participants;

        return _mapper.Map<IEnumerable<PlayerDto>>(players);

    }
}
