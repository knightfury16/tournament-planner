using AutoMapper;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Application.Request;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Exceptions;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application.RequestHandler;

public class GetMatchRequestHandler : IRequestHandler<GetMatchRequest, MatchDto>
{
    private readonly IRepository<Match> _matchRepository;
    private readonly IMapper _mapper;

    public GetMatchRequestHandler(IRepository<Match> matchRepository, IMapper mapper)
    {
        _matchRepository = matchRepository;
        _mapper = mapper;
    }
    public async Task<MatchDto?> Handle(GetMatchRequest request, CancellationToken cancellationToken = default)
    {
        if(request == null )throw new ArgumentException(nameof(request));

        var match = await _matchRepository.GetByIdAsync(request.MatchId, [nameof(Match.FirstPlayer), nameof(Match.SecondPlayer)]);
        if(match == null)throw new NotFoundException(nameof(match));

        return _mapper.Map<MatchDto>(match);
    }
}
