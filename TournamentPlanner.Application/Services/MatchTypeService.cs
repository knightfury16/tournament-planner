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

    public MatchTypeService(ICreateMatchTypeFactory createMatchTypeFactory, IRepository<MatchType> matchTypeRepository, IRepository<Draw> drawRepository)
    {
        this.createMatchTypeFactory = createMatchTypeFactory;
        _matchTypeRepository = matchTypeRepository;
        _drawRepository = drawRepository;
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
            //this is the second draw of the tournament, the second draw the last that means previous draw was group
            playersToCreateMatchType = await GetMatchTypeParticipants(tournament.Draws);
            //group stage will be only one time,all other knockout
            matchTypeCreator = createMatchTypeFactory.GetMatchTypeCreator(TournamentType.Knockout);
        }

        var matchTypes = await matchTypeCreator.CreateMatchType(tournament, playersToCreateMatchType, matchTypePrefix, seeders);

        if (matchTypes == null) throw new Exception("Could not make match types from tournament service");
        return matchTypes;
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
    private async Task<List<Player>> GetMatchTypeParticipants(List<Draw> draws)
    {
        // I dont need to check the winner of the prvious knocout match , maybe i need it while scheduling but not now
        // draws = draws.OrderByDescending(d => d.CreatedAt).ToList();
        // //not the first knockout
        // if (draws.FirstOrDefault()?.MatchType is KnockOut) // latest knockout, sorted by date
        // {
        //     return await GetWinnerOfPreviousKnockOut(draws.First());
        // }
        //it is first knockout match
        await Task.CompletedTask;
        return GetGroupStanding(draws);
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

    private List<Player> GetGroupStanding(List<Draw> draws)
    {
        throw new NotImplementedException();
    }
}
