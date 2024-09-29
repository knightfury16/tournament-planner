using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.Services;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Exceptions;
using MatchType = TournamentPlanner.Domain.Entities.MatchType;

namespace TournamentPlanner.Application;

public interface IRoundService
{
    public Task UpdateRoundCompletion(Round round);
    public Task<bool> IsAllRoundComplete(MatchType matchType);
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

    public async Task<bool> IsAllRoundComplete(MatchType matchType)
    {
        if(matchType == null)throw new ArgumentNullException(nameof(matchType));

        var rounds = matchType.Rounds;

        if(rounds == null){
            rounds = (await _roundRepository.GetAllAsync(r => r.MatchTypeId == matchType.Id)).ToList();
        }

        if(rounds == null || rounds.Count == 0){
            return true; //since no round is there so all round is complete
        }

        foreach (var round in rounds)
        {
            if(round.IsCompleted == false)return false;// if any round is not completed than all round is not completed
        }

        // if here then all round is complete
        return true;
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
