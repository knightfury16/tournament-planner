using AutoMapper;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Application.Helpers;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Exceptions;
using TournamentPlanner.Mediator;
using MatchType = TournamentPlanner.Domain.Entities.MatchType;

namespace TournamentPlanner.Application;

//! Getting the tournament matches via the Draws fully populated
public class GetTournamentMatchesRequestHandler : IRequestHandler<GetTournamentMatchesRequest, IEnumerable<DrawDto>>
{
    private readonly IRepository<Tournament> _tournamentRepository;
    private readonly IMapper _mapper;

    public GetTournamentMatchesRequestHandler(IRepository<Tournament> tournamentRepository, IMapper mapper)
    {
        _tournamentRepository = tournamentRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DrawDto>?> Handle(GetTournamentMatchesRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var navProp = Utility.NavigationPrpertyCreator(nameof(Tournament.Draws),nameof(Draw.MatchType),nameof(MatchType.Rounds));
        var tournament = await _tournamentRepository.GetByIdAsync(request.Id, [nameof(Tournament.Matches), nameof(Tournament.Participants), navProp]);
        if (tournament == null)throw new NotFoundException(nameof(Tournament));

        var draws = tournament.Draws; //fully populated


        // Reducing payload size
        return _mapper.Map<IEnumerable<DrawDto>>(draws.Select(d => {d.Tournament = null!;return d;}));

    }
}
