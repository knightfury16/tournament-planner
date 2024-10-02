using AutoMapper;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.Helpers;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Exceptions;
using TournamentPlanner.Mediator;
using MatchType = TournamentPlanner.Domain.Entities.MatchType;

namespace TournamentPlanner.Application;

public class GetMatchTypeRequestHandler : IRequestHandler<GetMatchTypeRequest, MatchTypeDto>
{

    private readonly IRepository<MatchType> _matchTypeRepository;
    private readonly IRepository<Round> _roundRepository;
    private readonly IMapper _mapper;

    public GetMatchTypeRequestHandler(IRepository<MatchType> matchTypeRepository, IMapper mapper, IRepository<Round> roundRepository)
    {
        _matchTypeRepository = matchTypeRepository;
        _mapper = mapper;
        _roundRepository = roundRepository;
    }



    public async Task<MatchTypeDto?> Handle(GetMatchTypeRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        // get the matchtype and populate the round and matches
        var roundNavigationProop = Utility.NavigationPrpertyCreator(nameof(MatchType.Rounds), nameof(Round.Matches));
        var matchType = await _matchTypeRepository.GetByIdAsync(request.MatchTypeId, [nameof(MatchType.Players),roundNavigationProop]);

        if (matchType == null) throw new NotFoundException(nameof(matchType));

        return _mapper.Map<MatchTypeDto>(matchType);


    }
}
