using TournamentPlanner.Application.Common;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Application.Helpers;
using TournamentPlanner.Domain;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Enum;
using TournamentPlanner.Domain.Exceptions;
using MatchType = TournamentPlanner.Domain.Entities.MatchType;

namespace TournamentPlanner.Application.Services;
public interface IMatchService
{
    public bool IsMatchComplete(Match match);
    public Task<IEnumerable<Match>> CreateMatches(Tournament tournament, SchedulingInfo schedulingInfo);
}
public class MatchService : IMatchService
{
    private readonly IRoundRobin _rounRobin;
    private readonly IKnockout _knockOut;
    private readonly IGameFormatFactory _gameFormatFactory;

    private readonly IRepository<Draw> _drawRepository;
    private readonly IRepository<MatchType> _matchTypeRepository;
    private readonly IMatchScheduler _matchScheduler;

    public MatchService(IRepository<Draw> drawRepository, IRepository<MatchType> matchTypeRepository, IMatchScheduler matchScheduler, IRoundRobin rounRobin, IGameFormatFactory gameFormatFactory, IKnockout knockOut)
    {
        _drawRepository = drawRepository;
        _matchTypeRepository = matchTypeRepository;
        _matchScheduler = matchScheduler;
        _rounRobin = rounRobin;
        _gameFormatFactory = gameFormatFactory;
        _knockOut = knockOut;
    }

    public async Task<IEnumerable<Match>> CreateMatches(Tournament tournament, SchedulingInfo? schedulingInfo)
    {
        var createdMatches = new List<Match>();
        //if tournament current state knockout  create knocout match
        if (tournament.CurrentState == TournamentState.KnockoutState)
        {
            createdMatches = await CreateKnockOutMatches(tournament);
        }
        //if tournament current state group create matches in the group
        if (tournament.CurrentState == TournamentState.GroupState)
        {
            createdMatches = await CreateGroupMatches(tournament);
        }

        var scheduledMatches = _matchScheduler.DefaultMatchScheduler(ref createdMatches, schedulingInfo!);

        return scheduledMatches;
    }

    private async Task<List<Match>> CreateGroupMatches(Tournament tournament)
    {
        //- I presume all the match type of draws are of the same type
        List<Match> createdMatches = new List<Match>();
        var draws = tournament.Draws;
        foreach (var draw in draws)
        {
            var matches = await _rounRobin.CreateMatches(tournament, draw.MatchType);
            createdMatches.AddRange(matches);
        }
        return createdMatches;
    }

    private async Task<List<Match>> CreateKnockOutMatches(Tournament tournament)
    {
        List<Match> createdMatches = new List<Match>();
        //in each tournament i will only have one knockout draw
        var allRoundWithMatch = Utility.NavigationPrpertyCreator(nameof(Draw.MatchType), nameof(MatchType.Rounds), nameof(Round.Matches));
        var allPlayersProperty = Utility.NavigationPrpertyCreator(nameof(Draw.MatchType), nameof(MatchType.Players));

        var knockoutDraw = tournament.Draws.Where(draw => draw.MatchType is KnockOut).FirstOrDefault();
        if (knockoutDraw == null) throw new NotFoundException("Could not find knockout draw to create matches");
        await _drawRepository.GetByIdAsync(knockoutDraw.Id, [allRoundWithMatch]); //populating the knockout draw

        //check if it is the first round
        if (knockoutDraw.MatchType.Rounds.Count == 0)
        {
            //is the tournamnent type knockout
            if (tournament.TournamentType == TournamentType.Knockout)
            {
                //go make matches with the player
                createdMatches = (List<Match>)await _knockOut.CreateFirstRoundMatches(tournament, knockoutDraw.MatchType);
            }
            else
            {

                var groupDraws = tournament.Draws.Where(draw => draw.MatchType is Group);
                var gameTypeHandler = _gameFormatFactory.GetGameFormat(tournament.GameType.Name);
                Dictionary<string, List<PlayerStanding>> groupOfPlayerStanding = new Dictionary<string, List<PlayerStanding>>();
                foreach (var groupDraw in groupDraws)
                {
                    await _drawRepository.GetByIdAsync(groupDraw.Id, [allPlayersProperty, allRoundWithMatch]); //populating the draw
                    var groupStanding = gameTypeHandler.GetGroupStanding(tournament, groupDraw.MatchType);
                    if (groupStanding == null) throw new Exception("Group stading is null. could not determine group stanidng");
                    groupOfPlayerStanding.Add(groupDraw.MatchType.Name, groupStanding);
                }

                createdMatches = (List<Match>)await _knockOut.CreateFirstRoundMatchesAfterGroup(tournament, knockoutDraw.MatchType, groupOfPlayerStanding);
            }

        }
        //else create subsequent matches of the knockout
        else
        {
            createdMatches = (List<Match>)await _knockOut.CreateSubsequentMatches(tournament, knockoutDraw.MatchType);
        }

        return createdMatches;
    }

    public bool IsMatchComplete(Match match)
    {
        if(match == null)throw new ArgumentNullException(nameof(match));

        //decide on the score property if the match is complete or not
        if(match.ScoreJson != null)return true;
        return false;
    }
}
