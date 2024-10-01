using TournamentPlanner.Application.Common;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.Helpers;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Enum;
using TournamentPlanner.Domain.Exceptions;
using MatchType = TournamentPlanner.Domain.Entities.MatchType;

namespace TournamentPlanner.Application;
public interface IMatchTypeService
{
    public Task<IEnumerable<MatchType>> CreateMatchType(Tournament tournament, string? matchTypePrefix = null, List<int>? seeders = null);
    public Task UpdateMatchTypeCompletion(MatchType matchType);
}
public class MatchTypeService : IMatchTypeService
{
    private readonly ICreateMatchTypeFactory createMatchTypeFactory;
    private readonly IRepository<MatchType> _matchTypeRepository;
    private readonly IRepository<Draw> _drawRepository;
    private readonly IRepository<Tournament> _tournamentRepository;
    private readonly IGameFormatFactory _gameFormatFactory;

    public MatchTypeService(ICreateMatchTypeFactory createMatchTypeFactory, IRepository<MatchType> matchTypeRepository, IRepository<Draw> drawRepository, ICreateMatchFactory matchCreatorFactory, IGameFormatFactory gameFormatFactory, IRepository<Tournament> tournamentRepository)
    {
        this.createMatchTypeFactory = createMatchTypeFactory;
        _matchTypeRepository = matchTypeRepository;
        _drawRepository = drawRepository;
        _gameFormatFactory = gameFormatFactory;
        _tournamentRepository = tournamentRepository;
    }

    public async Task<IEnumerable<MatchType>> CreateMatchType(Tournament tournament, string? matchTypePrefix = null, List<int>? seeders = null)
    {
        //previous draws exists?
        var playersToCreateMatchType = new List<Player>();
        ICreateMatchType matchTypeCreator;

        if (tournament.Draws == null || tournament.Draws.Count == 0)
        {
            //if it is the first draw of the 2 according to the axiom, let the tournament type handle it
            playersToCreateMatchType = tournament.Participants;
            matchTypeCreator = createMatchTypeFactory.GetMatchTypeCreator(tournament.TournamentType ?? TournamentType.GroupStage);
        }
        else
        {
            //this is the second draw of the tournament,  that means previous draw was group
            //! Change the  tournament current state here
            //if im here and tournament state is not GroupState, that means something went wrong
            if (tournament.CurrentState != TournamentState.GroupState) throw new BadRequestException($"Could not create match type form {nameof(CreateMatchType)} service");
            tournament.CurrentState = TournamentState.KnockoutState;
            playersToCreateMatchType = await GetGroupStandingPlayers(tournament);

            //group stage will be only one time,all other knockout
            matchTypeCreator = createMatchTypeFactory.GetMatchTypeCreator(TournamentType.Knockout);
        }

        var matchTypes = await matchTypeCreator.CreateMatchType(tournament, playersToCreateMatchType, matchTypePrefix, seeders);

        if (matchTypes == null) throw new Exception("Could not make match types from tournament service");
        return matchTypes;
    }
    private async Task<List<Player>> GetGroupStandingPlayers(Tournament tournament)
    {
        var advancePlayers = new List<Player>();
        //check if game type is null or else fetch it
        if(tournament.GameType == null)
        {
            await _tournamentRepository.ExplicitLoadReferenceAsync(tournament, t => t.GameType);
        }
        //group standing calculation will be game type specific
        var gameTypeHandler = _gameFormatFactory.GetGameFormat(tournament.GameType!.Name);
        foreach (var draw in tournament.Draws)
        {
            var allPlayersProperty = Utility.NavigationPrpertyCreator(nameof(Draw.MatchType), nameof(MatchType.Players));
            var allRoundWithMatch = Utility.NavigationPrpertyCreator(nameof(Draw.MatchType), nameof(MatchType.Rounds), nameof(Round.Matches));
            await _drawRepository.GetByIdAsync(draw.Id, [allPlayersProperty, allRoundWithMatch]);
            var playerStandings = gameTypeHandler.GetGroupStanding(tournament, draw.MatchType);
            advancePlayers.AddRange(playerStandings.Select(ps => ps.Player));
        }
        await Task.CompletedTask;
        return advancePlayers;
    }

    public async Task UpdateMatchTypeCompletion(MatchType matchType)
    {
        if (matchType == null) throw new ArgumentNullException(nameof(matchType));

        var matchTypeWithPopulatedRound = await _matchTypeRepository.GetByIdAsync(matchType.Id, [nameof(MatchType.Rounds)]);
        if (matchTypeWithPopulatedRound == null) throw new NotFoundException(nameof(matchTypeWithPopulatedRound));
        var rounds = matchTypeWithPopulatedRound.Rounds;
        foreach (var round in rounds)
        {
            //if any round is not complete return
            if (!round.IsCompleted) return;
        }
        //if here then all round complete
        matchType.IsCompleted = true;
        await _matchTypeRepository.SaveAsync();
    }
   

    private async Task<List<Player>> GetWinnerOfPreviousKnockOut(Draw draw)
    {
        var navigationProp = Utility.NavigationPrpertyCreator(nameof(Draw.MatchType), nameof(MatchType.Rounds), nameof(Round.Matches), nameof(Match.Winner));

        var drawPopulated = await _drawRepository.GetByIdAsync(draw.Id, [navigationProp]);

        if (drawPopulated == null) throw new NotFoundException(nameof(draw));

        var matches = drawPopulated.MatchType.Rounds.First().Matches;

        var players = matches.Select(m => m.Winner).ToList();

        if (players == null) throw new Exception("Could not create winner player list");

        return players!;
    }

}
