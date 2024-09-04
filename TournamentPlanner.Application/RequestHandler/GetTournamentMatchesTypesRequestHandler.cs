using AutoMapper;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class GetTournamentMatchesTypesRequestHandler : IRequestHandler<GetTournamentMatchTypesRequest, IEnumerable<MatchTypeDto>>
{
    private readonly IRepository<Tournament> _tournamentRepository;
    private readonly IMapper _mapper;

    public GetTournamentMatchesTypesRequestHandler(IRepository<Tournament> tournamentRepository, IMapper mapper)
    {
        _tournamentRepository = tournamentRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MatchTypeDto>?> Handle(GetTournamentMatchTypesRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var matchTypes = (await _tournamentRepository.GetAllAsync(t => t.Id == request.Id, [nameof(Tournament.MatchTypes)]))
                    .SelectMany(t => t.MatchTypes);

        return _mapper.Map<IEnumerable<MatchTypeDto>>(matchTypes);

    }
}
