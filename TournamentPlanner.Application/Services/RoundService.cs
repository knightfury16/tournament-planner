using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.Services;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Exceptions;

namespace TournamentPlanner.Application;

public interface IRoundService
{
    public Task UpdateRoundCompletion(Round round);
}
public class RoundService : IRoundService
{
    private readonly IRepository<Round> _roundRepository;
    private readonly IMatchService _matchService;
    private readonly IMatchTypeService _matchTypeService;

    public RoundService(IRepository<Round> roundRepository, IMatchService matchService, IMatchTypeService matchTypeService)
    {
        _roundRepository = roundRepository;
        _matchService = matchService;
        _matchTypeService = matchTypeService;
    }

    public async Task UpdateRoundCompletion(Round round)
    {
        //get all the matches of the round
        var roundWithPopulatedMatches = await _roundRepository.GetByIdAsync(round.Id, [nameof(Round.Matches), nameof(Round.MatchType)]);

        if (roundWithPopulatedMatches == null)throw new NotFoundException(nameof(roundWithPopulatedMatches));

        var matches = roundWithPopulatedMatches.Matches;

        foreach (var match in matches)
        {
            //if any match is not complete round is not complete
            if(!_matchService.IsMatchComplete(match)){
                return;
            }
        }

        //if here then all matches are complete\
        round.IsCompleted = true;
        await _roundRepository.SaveAsync();
        //call the update matchtype
        await _matchTypeService.UpdateMatchTypeCompletion(round.MatchType);

    }
}
