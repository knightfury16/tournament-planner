using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.Helpers;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Enum;
using MatchType = TournamentPlanner.Domain.Entities.MatchType;

namespace TournamentPlanner.Application;

public interface ITournamentService
{
    public Task<bool> CanIMakeDraw(Tournament tournament);
    public Task<IEnumerable<Draw>> MakeDraws(
        Tournament tournament,
        string? matchTypePrefix = null,
        List<int>? seederPlayers = null
    );
    public Task<bool> CanISchedule(Tournament tournament);
    public bool AmITheCreator(Tournament tournament);
    public Task<bool> ChangeTournamentStatus(
        Tournament tournament,
        TournamentStatus requestedStatus
    );
    public Task<bool> ChangeTournamentStatus(string tournamentId, TournamentStatus requestedStatus);
}

public class TournamentService : ITournamentService
{
    private readonly IDrawService _drawService;
    private readonly IMatchTypeService _matchTypeService;
    private readonly IRoundService _roundService;
    private readonly IRepository<Tournament> _tournamentRepository;
    private readonly IRepository<MatchType> _matchTypeRepository;
    private readonly ICurrentUser _currentUser;

    public TournamentService(
        IDrawService drawService,
        IMatchTypeService matchTypeService,
        IRepository<Tournament> tornamentRepository,
        IRoundService roundService,
        ICurrentUser currentUser,
        IRepository<MatchType> matchTypetRepository
    )
    {
        this._drawService = drawService;
        _matchTypeService = matchTypeService;
        _tournamentRepository = tornamentRepository;
        _roundService = roundService;
        _currentUser = currentUser;
        _matchTypeRepository = matchTypetRepository;
    }

    public async Task<bool> CanIMakeDraw(Tournament tournament)
    {
        //TODO: Need to check tournament status here. if tournament status is complete can not make draw even if all the draw are complete
        // i dont need to check the status. once a tournament is on knockoutstate it can not draw

        //no draws availabe, inital state
        if (tournament.Draws == null || tournament.Draws.Count == 0)
            return true;

        //draws exists and state is not knockout
        if (tournament.CurrentState == TournamentState.GroupState)
            return await _drawService.IsDrawsComplete(tournament);

        //in all cases it is false
        return false;
    }

    public async Task<bool> CanISchedule(Tournament tournament)
    {
        var draws = tournament.Draws;
        if (draws == null) //populate the draws if null
        {
            var tournamentDrawPopulated = await _tournamentRepository.GetByIdAsync(
                tournament.Id,
                [nameof(Tournament.Draws)]
            );
            draws = tournamentDrawPopulated?.Draws;
        }

        if (draws == null)
            throw new NullReferenceException(nameof(draws));
        if (draws.Count == 0)
            return false; // i have not made any draw yet

        // double checking me I dont beleive in me. alright i beleive but little
        if (tournament.Matches == null || tournament.Matches.Count == 0)
        {
            await _tournamentRepository.ExplicitLoadCollectionAsync(tournament, t => t.Matches);
        }

        //this will only his in initial state of Group Matches
        if (tournament.Matches == null || tournament.Matches.Count == 0)
            return true; // i have made draw but no matches scheduled yet

        if (tournament.CurrentState == TournamentState.KnockoutState)
        {
            var knockoutDraw = draws.Where(d => d.MatchType is KnockOut).FirstOrDefault();
            if (knockoutDraw == null)
                throw new NullReferenceException(
                    "ToournamentStatus is knokout but could not find any knockout draw"
                );
            //for knockout matches in order to know if i can make schedule i need to check if all the previous roudn of the \
            //knockout is finsihed or not
            //for knockout matches i dont need it to be complete, i just need to know if the previous round is complete or not
            //the reason is that, for knockout match type i can not scheudle all the matches of all the round at onece
            //i need to know the winner of the previous round in order to schedule the next round
            //in group match type i know all the matches of all the round before hand so in order to schedule it i dont need to
            //to know any information beforehand
            return await IsAllRoundOfKnockoutMatchtypeComplete(knockoutDraw.MatchType);
        }

        return false; //in all other cases it is false
    }

    private async Task<bool> IsAllRoundOfKnockoutMatchtypeComplete(MatchType matchType)
    {
        if (matchType == null)
            throw new ArgumentNullException(nameof(matchType));

        //load all the rounds
        if (matchType.Rounds.Count == 0)
            await _matchTypeRepository.ExplicitLoadCollectionAsync(matchType, mt => mt.Rounds);

        //see if final round exists
        var finalRound = matchType.Rounds.Where(r => r.RoundName == Utility.Final).FirstOrDefault();

        if (finalRound == null)
            return await _roundService.IsAllRoundComplete(matchType);

        //if here then final round found and if once final round is created
        //i can not schedule any more matches coz there is noting to schedule. So returning false
        return false;
    }

    public async Task<IEnumerable<Draw>> MakeDraws(
        Tournament tournament,
        string? matchTypePrefix = null,
        List<int>? seedersPlayers = null
    )
    {
        ArgumentNullException.ThrowIfNull(tournament);


        var areSeedersValid = ValidateSeeders(tournament, seedersPlayers);
        if (!areSeedersValid)
            throw new Exception("Seeders are not valid");
        var matchTypes = await _matchTypeService.CreateMatchType(
            tournament,
            matchTypePrefix,
            seedersPlayers
        );
        var draws = matchTypes.Select(mt => GetDraw(mt, tournament));


        //before procedding further need to change the tournament to ongoing if it is not already changed
        //After making a draw there is no point in keeeping the tournament status to open.
        //Making Draw automatically ensure that tournament has started
        //putting this at the end so that it can catch any error before
        await ChangeTournamentStatus(tournament, TournamentStatus.Ongoing);

        return draws;
    }

    private bool ValidateSeeders(Tournament tournament, List<int>? seedersPlayers)
    {
        if (seedersPlayers == null)
            return true; //seeders not seeded
        if (seedersPlayers.Count > tournament.Participants.Count)
            return false;
        return tournament.Participants.Select(p => p.Id).Intersect(seedersPlayers).Count()
            == seedersPlayers.Count();
    }

    private Draw GetDraw(Domain.Entities.MatchType mt, Tournament tournament)
    {
        return new Draw { Tournament = tournament, MatchType = mt };
    }

    public bool AmITheCreator(Tournament tournament)
    {
        if (_currentUser.DomainUserId == null)
            return false;
        if (tournament == null)
            return false;
        if (tournament.AdminId == 0)
            return false;
        if (tournament.AdminId != _currentUser.DomainUserId)
            return false;
        if (tournament.AdminId == _currentUser.DomainUserId)
            return true;

        //default false
        return false;
    }

    public async Task<bool> ChangeTournamentStatus(
        Tournament tournament,
        TournamentStatus requestedStatus
    )
    {
        ArgumentNullException.ThrowIfNull(tournament);

        return await ChangeStatus(tournament, requestedStatus);
    }

    private async Task<bool> ChangeStatus(Tournament tournament, TournamentStatus requestedStatus)
    {
        var currentStatus =
            tournament.Status
            ?? throw new InvalidOperationException("Tournament status is unexpectedly null.");
        //I can change between Draft, RegistrationOpen, RegistrationClosed back and forth as much as i want
        //but once status is ongoing i cant go back

        //check if the requestedStatus the same as currentStatus. if so then no need to change, return true
        if (currentStatus == requestedStatus)
            return true;

        if (currentStatus == TournamentStatus.Completed)
            return false;

        if (
            currentStatus >= TournamentStatus.Ongoing
            && requestedStatus <= TournamentStatus.Ongoing
        )
        {
            return (false);
        }

        tournament.Status = requestedStatus;
        await _tournamentRepository.SaveAsync();
        return true;
    }

    public async Task<bool> ChangeTournamentStatus(
        string tournamentId,
        TournamentStatus requestedStatus
    )
    {
        ArgumentNullException.ThrowIfNullOrEmpty(tournamentId);
        // Try parsing the tournamentId safely
        if (int.TryParse(tournamentId, out int parsedTournamentId))
        {
            // Successfully parsed, proceed with logic
            var tournament = await _tournamentRepository.GetByIdAsync(parsedTournamentId);
            ArgumentNullException.ThrowIfNull(tournament);

            return await ChangeStatus(tournament, requestedStatus);
        }
        else
        {
            // Handle invalid tournamentId case
            throw new ArgumentException("Invalid Tournament ID format.", nameof(tournamentId));
        }
    }
}
