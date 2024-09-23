using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Enum;

namespace TournamentPlanner.Application;
public interface ITournamentService
{
    public Task<bool> CanIMakeDraw(Tournament tournament);
    public Task<IEnumerable<Draw>> MakeDraws(Tournament tournament, string? matchTypePrefix = null, List<int>? seederPlayers = null);
}

public class TournamentService : ITournamentService
{
    private readonly IDrawService _drawService;
    private readonly IMatchTypeService _matchTypeService;

    public TournamentService(IDrawService drawService, IMatchTypeService matchTypeService)
    {
        this._drawService = drawService;
        _matchTypeService = matchTypeService;
    }

    public async Task<bool> CanIMakeDraw(Tournament tournament)
    {

        if (tournament.Draws != null && tournament.Draws.Count == 0) return true;
        return await _drawService.IsTheDrawComplete(tournament.Draws!);//checked null top line

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
