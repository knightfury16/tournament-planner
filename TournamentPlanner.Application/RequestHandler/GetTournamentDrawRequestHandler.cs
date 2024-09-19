using AutoMapper;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.Request;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application.RequestHandler;

public class GetTournamentDrawRequestHandler : IRequestHandler<GetTournamentDrawRequest, IEnumerable<DrawDto>>
{
    private readonly IRepository<Draw> _drawRepository;
    private readonly IMapper _mapper;
    public GetTournamentDrawRequestHandler(IRepository<Draw> drawRepository, IMapper mapper)
    {
        _drawRepository = drawRepository;
        _mapper = mapper;
    }
    public async Task<IEnumerable<DrawDto>?> Handle(GetTournamentDrawRequest request, CancellationToken cancellationToken = default)
    {
        var navigationString = nameof(Draw.MatchType)+ "." + nameof(Domain.Entities.MatchType.Players);
        var draws = await _drawRepository.GetAllAsync(d => d.TournamentId == request.TournamentId, [navigationString]);
        return _mapper.Map<IEnumerable<DrawDto>>(draws);
    }
}
