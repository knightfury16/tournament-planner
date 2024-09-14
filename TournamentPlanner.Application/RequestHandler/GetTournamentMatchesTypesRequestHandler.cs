using AutoMapper;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class GetTournamentMatchesTypesRequestHandler : IRequestHandler<GetTournamentMatchTypesRequest, IEnumerable<MatchTypeDto>>
{
    private readonly IRepository<Draw> _drawRepository;
    private readonly IMapper _mapper;

    public GetTournamentMatchesTypesRequestHandler(IRepository<Draw> drawRepository, IMapper mapper)
    {
        _drawRepository = drawRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MatchTypeDto>?> Handle(GetTournamentMatchTypesRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var matchTypes =(await _drawRepository.GetAllAsync(t => t.Id == request.tournamentId,[nameof(Draw.MatchType)])).Select(d => d.MatchType).ToList();

        return _mapper.Map<IEnumerable<MatchTypeDto>>(matchTypes);

    }
}
