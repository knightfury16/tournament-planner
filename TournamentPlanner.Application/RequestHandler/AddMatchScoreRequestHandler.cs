using AutoMapper;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Application.Helpers;
using TournamentPlanner.Application.Request;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Enum;
using TournamentPlanner.Domain.Exceptions;
using TournamentPlanner.Domain.Interface;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application.RequestHandler;

public class AddMatchScoreRequestHandler : IRequestHandler<AddMatchScoreRequest, MatchDto>
{
    private readonly IRepository<Match> _matchRepository;
    private readonly IMapper _mapper;
    private readonly IGameFormatFactory _gameFormatFactory;
    private readonly IRoundService _roundService;
    private readonly ITournamentService _tournamentService;

    public AddMatchScoreRequestHandler(IRepository<Match> matchRepository, IMapper mapper, IGameFormatFactory gameFormatFactory, IRoundService roundService, ITournamentService tournamentService)
    {
        _matchRepository = matchRepository;
        _mapper = mapper;
        _gameFormatFactory = gameFormatFactory;
        _roundService = roundService;
        _tournamentService = tournamentService;
    }


    //-- I can not be on this handler if im not an admin
    public async Task<MatchDto?> Handle(AddMatchScoreRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        //get the match, need palyers and tournament game type
        var tournamentNavigationProp = Utility.NavigationPrpertyCreator(nameof(Match.Tournament), nameof(Tournament.GameType));
        var match = await _matchRepository.
                                GetByIdAsync(request.MatchId, [tournamentNavigationProp,
                                nameof(Match.FirstPlayer), nameof(Match.SecondPlayer), nameof(Match.Round)]);

        if (match == null) throw new NotFoundException(nameof(match));

        if (!_tournamentService.AmITheCreator(match.Tournament)) throw new AdminOwnershipException();

        //get the game type handler
        var gameTypeHandler = _gameFormatFactory.GetGameFormat(match.Tournament.GameType.Name);
        if (gameTypeHandler == null) throw new NotFoundException($"Game type handler could not be resolved");

        //use the handler to validate the score
        IScore gameScore = gameTypeHandler.DeserializeScore(request.AddMatchScoreDto.GameSpecificScore);
        if (!gameTypeHandler.IsValidScore(gameScore))
        {
            throw new ValidationException("Invalid game-specific data for the given game type.");
        }

        //determine the winner
        var winner = gameTypeHandler.DetermineWinner(match.FirstPlayer, match.SecondPlayer, gameScore);//specify the first player and second player in thhe param, according to gameScore player1 and player2

        //update the match and player stats here
        UpdateMatchAndPlayerStat(winner, gameTypeHandler.SerializeScore(gameScore), request.AddMatchScoreDto.GamePlayed, ref match);
        await _matchRepository.SaveAsync();

        //if i could send and event and let it complete that would be ideal here.
        //but this is a small app, will not have problem

        //update the round is complete or not
        await _roundService.UpdateRoundCompletion(match.Round);


        return _mapper.Map<MatchDto>(match);

    }

    private void UpdateMatchAndPlayerStat(Player winner, string serializedScore, DateTime? gamePlayed, ref Match match)
    {
        //update match 
        match.Winner = winner;
        match.GamePlayed = gamePlayed ?? match.GameScheduled; // if game played is not provided then fall back to tournament scheduled date
        match.ScoreJson = serializedScore;

        //update the game played stat
        match.FirstPlayer.GamePlayed += 1;
        match.SecondPlayer.GamePlayed += 1;

        //update the game won
        if (match.FirstPlayer.Id == winner.Id) match.FirstPlayer.GameWon += 1;
        else match.SecondPlayer.GameWon += 1;

    }
}
