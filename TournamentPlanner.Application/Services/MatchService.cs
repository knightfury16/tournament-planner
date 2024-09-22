using TournamentPlanner.Application.Common;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Domain.Entities;
using MatchType = TournamentPlanner.Domain.Entities.MatchType;

namespace TournamentPlanner.Application.Services;
public interface IMatchService 
{
    public Task<IEnumerable<Match>> CreateMatches(Tournament tournament, SchedulingInfo schedulingInfo);
}
public class MatchService : IMatchService
{
    private readonly ICreateMatchFactory _matchCreatorFactory;
    private readonly IRepository<Draw> _drawRepository;
    private readonly IRepository<MatchType> _matchTypeRepository;
    private readonly IMatchScheduler _matchScheduler;

    public MatchService(ICreateMatchFactory matchCreatorFactory, IRepository<Draw> drawRepository, IRepository<MatchType> matchTypeRepository, IMatchScheduler matchScheduler)
    {
        _matchCreatorFactory = matchCreatorFactory;
        _drawRepository = drawRepository;
        _matchTypeRepository = matchTypeRepository;
        _matchScheduler = matchScheduler;
    }

    public async Task<IEnumerable<Match>> CreateMatches(Tournament tournament, SchedulingInfo? schedulingInfo)
    {
        //!i presume all the match type of draws are of the same type
        var draws = tournament.Draws;
        var matchType = draws.First().MatchType;
        var matchCreator = _matchCreatorFactory.GetMatchCreator(matchType);
        var createdMatches = new List<Match>();
        foreach (var draw in draws){
            //!!need to schedule time properly
            var matches = await matchCreator.CreateMatches(tournament, draw.MatchType);
            createdMatches.AddRange(matches);
        }

        var scheduledMatches = _matchScheduler.DefaultMatchScheduler(ref createdMatches, schedulingInfo!);

        return scheduledMatches;
    }
}
