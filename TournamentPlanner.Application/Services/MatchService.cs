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
    public Task<bool> IsMatchComplete(Match match);
    public Task<IEnumerable<Match>> CreateMatches(Tournament tournament, SchedulingInfo schedulingInfo);
}
public class MatchService : IMatchService
{
    private readonly IRoundRobin _rounRobin;
    private readonly IKnockout _knockOut;
    private readonly IGameFormatFactory _gameFormatFactory;
    private readonly IRepository<Draw> _drawRepository;
    private readonly IRepository<Match> _matchRepository;

    public MatchService(IRepository<Draw> drawRepository, IRepository<Match> matchRepository, IRoundRobin rounRobin, IGameFormatFactory gameFormatFactory, IKnockout knockOut)
    {
        _drawRepository = drawRepository;
        _matchRepository = matchRepository;
        _rounRobin = rounRobin;
        _gameFormatFactory = gameFormatFactory;
        _knockOut = knockOut;
    }

    public async Task<IEnumerable<Match>> CreateMatches(Tournament tournament, SchedulingInfo? schedulingInfo)
    {
        ArgumentNullException.ThrowIfNull(tournament);

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

        return createdMatches;
    }

    private async Task<List<Match>> CreateGroupMatches(Tournament tournament)
    {
        //- I presume all the match type of draws are of the same type
        List<Match> createdMatches = new List<Match>();
         
        var draws = tournament.Draws;
        ArgumentNullException.ThrowIfNull(draws);

        foreach (var draw in draws)
        {
            if(draw.MatchType == null)throw new ArgumentNullException(nameof(draw.MatchType));
            if(draw.MatchType is KnockOut)throw new ValidationException("Found Knockout matchtype while creating group matches");

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
        var tournamentGameTypeProperty = Utility.NavigationPrpertyCreator(nameof(Draw.Tournament), nameof(Tournament.GameType));

        var knockoutDraw = tournament.Draws.Where(draw => draw.MatchType is KnockOut).FirstOrDefault();
        if (knockoutDraw == null) throw new NotFoundException("Could not find knockout draw to create matches");
        await _drawRepository.GetByIdAsync(knockoutDraw.Id, [allRoundWithMatch, tournamentGameTypeProperty]); //populating the knockout draw

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
                if (groupDraws == null || groupDraws.Count() == 0) throw new BadRequestException("Could not found Draws with Group match type");
                if (tournament.GameType == null) throw new BadRequestException("Could not determine Tournament Game Type");

                var gameTypeHandler = _gameFormatFactory.GetGameFormat(tournament.GameType!.Name); //game type cant be null here
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

    public async Task<bool> IsMatchComplete(Match match)
    {
        if (match == null) throw new ArgumentNullException(nameof(match));

        //check for player
        if (match.FirstPlayer == null || match.SecondPlayer == null)
        {
            //load player
            await _matchRepository.ExplicitLoadReferenceAsync(match, m => m.FirstPlayer);
            await _matchRepository.ExplicitLoadReferenceAsync(match, m => m.SecondPlayer);
        }

        if (match.FirstPlayer!.Name.ToLower().Contains("bye") && match.SecondPlayer!.Name.ToLower().Contains("bye"))
        {
            throw new Exception("Both players cannot be 'bye' in a match.");
        }


        if (match.FirstPlayer!.Name.ToLower().Contains("bye") || match.SecondPlayer!.Name.ToLower().Contains("bye"))
        {
            return true;
        }

        //decide on the score property if the match is complete or not
        if (match.ScoreJson != null) return true;

        return false;
    }
}
