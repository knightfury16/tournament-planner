using AutoMapper;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.Helpers;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Exceptions;
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
        var playerNavigationProperty = Utility.NavigationPrpertyCreator(nameof(Draw.MatchType), nameof(Domain.Entities.MatchType.Players));
        var roundNavigationProperty = Utility.NavigationPrpertyCreator(nameof(Draw.MatchType), nameof(Domain.Entities.MatchType.Rounds));
        var matchTypes = (await _drawRepository.GetAllAsync(d => d.TournamentId == request.tournamentId, [playerNavigationProperty, roundNavigationProperty])).Select(d => d.MatchType).ToList();

        if (matchTypes == null || matchTypes.Count == 0) throw new NotFoundException(nameof(matchTypes));

        return _mapper.Map<IEnumerable<MatchTypeDto>>(matchTypes);

    }
}
