using TournamentPlanner.Application.Common.Interfaces;
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

    public MatchTypeService(ICreateMatchTypeFactory createMatchTypeFactory, IRepository<MatchType> matchTypeRepository)
    {
        this.createMatchTypeFactory = createMatchTypeFactory;
        _matchTypeRepository = matchTypeRepository;
    }

    public async Task<IEnumerable<MatchType>> CreateMatchType(Tournament tournament, string? matchTypePrefix = null, List<int>? seeders = null)
    {
        var matchTypeCreator = createMatchTypeFactory.GetMatchTypeCreator(tournament.TournamentType ?? TournamentType.GroupStage);
        var matchTypes = await matchTypeCreator.CreateMatchType(tournament, matchTypePrefix, seeders);
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
}
