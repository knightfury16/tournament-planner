using AutoMapper;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Domain.Entities;
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
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var players = (await _tournamentRepository.GetAllAsync(t => t.Id == request.Id, [nameof(Tournament.Participants)]))
                    .SelectMany(t => t.Participants);

        return _mapper.Map<IEnumerable<PlayerDto>>(players);

    }
}
