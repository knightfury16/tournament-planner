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
}
public class MatchTypeService : IMatchTypeService
{
    private readonly ICreateMatchTypeFactory createMatchTypeFactory;
    private readonly IRepository<Draw> _drawRepository;

    public MatchTypeService(ICreateMatchTypeFactory createMatchTypeFactory, IRepository<Draw> drawRepository)
    {
        this.createMatchTypeFactory = createMatchTypeFactory;
        _drawRepository = drawRepository;
    }

    public async Task<IEnumerable<MatchType>> CreateMatchType(Tournament tournament, string? matchTypePrefix = null, List<int>? seeders = null)
    {
        //previous draws exists?
        var playersToCreateMatchType = new List<Player>();
        ICreateMatchType matchTypeCreator;
        if (tournament.Draws == null || tournament.Draws.Count == 0)
        {
            playersToCreateMatchType = tournament.Participants;
            matchTypeCreator = createMatchTypeFactory.GetMatchTypeCreator(tournament.TournamentType ?? TournamentType.GroupStage);
        }
        else
        {
            playersToCreateMatchType = await GetMatchTypeParticipants(tournament.Draws);
            //group stage will be only one time,all other knockout
            matchTypeCreator = createMatchTypeFactory.GetMatchTypeCreator(TournamentType.Knockout);
        }

        var matchTypes = await matchTypeCreator.CreateMatchType(tournament, playersToCreateMatchType, matchTypePrefix, seeders);

        if (matchTypes == null) throw new Exception("Could not make match types from tournament service");
        return matchTypes;
    }
    private async Task<List<Player>> GetMatchTypeParticipants(List<Draw> draws)
    {
        draws = draws.OrderByDescending(d => d.CreatedAt).ToList();

        //not the first knockout
        if (draws.FirstOrDefault()?.MatchType is KnockOut) // latest knockout, sorted by date
        {
            return await GetWinnerOfPreviousKnockOut(draws.First());
        }
        //it is first knockout match
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
