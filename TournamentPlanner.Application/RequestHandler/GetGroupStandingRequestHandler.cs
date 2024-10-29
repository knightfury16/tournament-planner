using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.Helpers;
using TournamentPlanner.Domain;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Exceptions;
using TournamentPlanner.Mediator;
using MatchType = TournamentPlanner.Domain.Entities.MatchType;

namespace TournamentPlanner.Application;

public class GetGroupStandingRequestHandler : IRequestHandler<GetGroupStandingRequest, IEnumerable<PlayerStanding>>
{
    private readonly IRepository<Tournament> _tournamentRepository;
    private readonly IRepository<MatchType> _matchTypeRepository;
    private readonly IGameFormatFactory _gameFormatFactory;

    public GetGroupStandingRequestHandler(IRepository<Tournament> tournamentRepository, IGameFormatFactory gameFormatFactory, IRepository<MatchType> matchTypeRepository)
    {
        _tournamentRepository = tournamentRepository;
        _gameFormatFactory = gameFormatFactory;
        _matchTypeRepository = matchTypeRepository;
    }

    public async Task<IEnumerable<PlayerStanding>?> Handle(GetGroupStandingRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        //populating matches of match type
        var includePropOfMatchType = Utility.NavigationPrpertyCreator(nameof(MatchType.Rounds), nameof(Round.Matches));
        var includePropTournament = Utility.NavigationPrpertyCreator(nameof(MatchType.Draw), nameof(Draw.Tournament));
        var matchType = await _matchTypeRepository.GetByIdAsync(request.GroupId, [includePropTournament, includePropOfMatchType]);

        if (matchType == null) throw new NotFoundException(nameof(matchType));
        if (matchType.Rounds == null || matchType.Rounds.Count == 0) throw new BadRequestException("No rounds founds to get group standing");

        var tournament = matchType.Draw?.Tournament;
        if (tournament == null) throw new NotFoundException(nameof(tournament));

        var gameTypeHandler = _gameFormatFactory.GetGameFormat(tournament.GameType.Name);

        if (gameTypeHandler == null) throw new NotFoundException($"Game type handler could not be resolved");

        var groupStanding = gameTypeHandler.GetGroupStanding(tournament, matchType, true);

        return groupStanding;
    }
}
