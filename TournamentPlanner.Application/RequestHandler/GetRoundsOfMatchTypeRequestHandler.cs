using AutoMapper;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Exceptions;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class GetRoundsOfMatchTypeRequestHandler : IRequestHandler<GetRoundsOfMatchTypeRequest, IEnumerable<RoundDto>>
{
    private readonly IRepository<Round> _roundRepository;
    private readonly IMapper _mapper;


    public GetRoundsOfMatchTypeRequestHandler(IRepository<Round> roundRepository, IMapper mapper)
    {
        _roundRepository = roundRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RoundDto>?> Handle(GetRoundsOfMatchTypeRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        //get round condition on mathctype id
        var rounds = await _roundRepository.GetAllAsync(r => r.MatchTypeId == request.MatchTypeId, [nameof(Round.Matches)]);

        if (rounds == null) throw new NotFoundException("Could not find any round of the ");

        return _mapper.Map<IEnumerable<RoundDto>>(rounds);

    }
}
