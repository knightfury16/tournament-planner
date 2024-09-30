using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Enum;

namespace TournamentPlanner.Application;
public interface ITournamentService
{
    public Task<bool> CanIMakeDraw(Tournament tournament);
    public Task<IEnumerable<Draw>> MakeDraws(Tournament tournament, string? matchTypePrefix = null, List<int>? seederPlayers = null);
    public Task<bool> CanISchedule(Tournament tournament);
}

public class TournamentService : ITournamentService
{
    private readonly IDrawService _drawService;
    private readonly IMatchTypeService _matchTypeService;
    private readonly IRoundService _roundService;
    private readonly IRepository<Tournament> _tournamentRepository;


    public TournamentService(IDrawService drawService, IMatchTypeService matchTypeService, IRepository<Tournament> tornamentRepository, IRoundService roundService)
    {
        this._drawService = drawService;
        _matchTypeService = matchTypeService;
        _tournamentRepository = tornamentRepository;
        _roundService = roundService;
    }

    public async Task<bool> CanIMakeDraw(Tournament tournament)
    {
        //TODO: Need to check tournament status here. if tournament status is complete can not make draw even if all the draw are complete
        // i dont need to check the status. once a tournament is on knockoutstate it can not draw

        //no draws availabe, inital state
        if(tournament.Draws == null || tournament.Draws.Count == 0)return true;

        //draws exists and state is not knockout
        if(tournament.CurrentState == TournamentState.GroupState)return await _drawService.IsTheDrawComplete(tournament);

        return false;
    }
    public async Task<bool> CanISchedule(Tournament tournament){
        var draws = tournament.Draws;
        if(draws == null){
            var tournamentDrawPopulated =  await _tournamentRepository.GetByIdAsync(tournament.Id, [nameof(Tournament.Draws)]);
            draws = tournamentDrawPopulated?.Draws;  
        }
        //sort the draws by created date
        if(draws == null)throw new NullReferenceException(nameof(draws));

        draws = draws.OrderByDescending(dr => dr.CreatedAt).ToList();

        if(draws.First().MatchType is KnockOut){
            //for knockout matches in order to know if i can make schedule i need to check if all the previous roudn of the \
            //knockout is finsihed or not 
            //for knockout matches i dont need it to be complete, i just need to know if the previous round is complete or not
            //the reason is that, for knockout match type i can not scheudle all the matches of all the round at onece
            //i need to know the winner of the previous round in order to schedule the next round
            //in group match type i know all the matches of all the round before hand so in order to schedule it i dont need to 
            //to know any information beforehand
            return await _roundService.IsAllRoundComplete(draws.First().MatchType);
        }

        //if it is  not knockout match type i need to check if all the preivious draws matchtype is complete or not
        //that is, is all the group matches competed
        return await _drawService.IsTheDrawComplete(draws);
    }
    public async Task<IEnumerable<Draw>> MakeDraws(Tournament tournament, string? matchTypePrefix = null, List<int>? seedersPlayers = null)
    {
        var areSeedersValid = ValidateSeeders(tournament, seedersPlayers);
        if(!areSeedersValid)throw new Exception("Seeders are not valid");
        //!!i need a match creator on match type param not only on tournament type.
        //!! because i can make draw, say group is finish, so next will be knockout
        var matchTypes = await _matchTypeService.CreateMatchType(tournament, matchTypePrefix, seedersPlayers);
        var draws = matchTypes.Select(mt => GetDraw(mt, tournament));
        return draws;
    }

    private bool ValidateSeeders(Tournament tournament, List<int>? seedersPlayers)
    {
        if(seedersPlayers == null)return true; //seeders not seeded
        return tournament.Participants.Select( p => p.Id).Intersect(seedersPlayers).Count() == seedersPlayers.Count();
    }

    private Draw GetDraw(Domain.Entities.MatchType mt, Tournament tournament)
    {
        return new Draw
        {
            Tournament = tournament,
            MatchType = mt
        };
    }
}
