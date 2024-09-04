using AutoMapper;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class GetTournamentMatchesRequestHandler:IRequestHandler<GetTournamentMatchesRequest, IEnumerable<MatchDto>>
{
    private readonly IRepository<Tournament> _tournamentRepository;
    private readonly IMapper _mapper;

    public GetTournamentMatchesRequestHandler(IRepository<Tournament> tournamentRepository, IMapper mapper)
    {
        _tournamentRepository = tournamentRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MatchDto>?> Handle(GetTournamentMatchesRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var matches = (await _tournamentRepository.GetAllAsync(t => t.Id == request.Id, [nameof(Tournament.Matches)]))
                    .SelectMany(t => t.Matches);

        return _mapper.Map<IEnumerable<MatchDto>>(matches);

    }
}
