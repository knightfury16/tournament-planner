using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Enum;
using MatchType = TournamentPlanner.Domain.Entities.MatchType;

namespace TournamentPlanner.Application;
public interface IMatchTypeService
{
    public Task<IEnumerable<MatchType>> CreateMatchType(Tournament tournament, string? matchTypePrefix = null, List<int>? seeders = null);
}
public class MatchTypeService : IMatchTypeService
{
    private readonly ICreateMatchTypeFactory createMatchTypeFactory;

    public MatchTypeService(ICreateMatchTypeFactory createMatchTypeFactory)
    {
        this.createMatchTypeFactory = createMatchTypeFactory;
    }

    public async Task<IEnumerable<MatchType>> CreateMatchType(Tournament tournament, string? matchTypePrefix = null, List<int>? seeders = null)
    {
        var matchTypeCreator = createMatchTypeFactory.GetMatchTypeCreator(tournament.TournamentType ?? TournamentType.GroupStage);
        var matchTypes = await matchTypeCreator.CreateMatchType(tournament, matchTypePrefix, seeders);
        if (matchTypes == null) throw new Exception("Could not make match types from tournament service");
        return matchTypes;
    }
}
