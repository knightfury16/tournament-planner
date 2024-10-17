using AutoMapper;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.GameTypeScore;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Exceptions;
using TournamentPlanner.Domain.Interface;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class SimulateMatchesRequestHandler : IRequestHandler<SimulateMatchesRequest, bool>
{

    private readonly IRepository<Match> _matchRepository;
    private readonly IMapper _mapper;
    private readonly IGameFormatFactory _gameFormatFactory;
    private readonly IRoundService _roundService;
    private readonly IRepository<Tournament> _tournamentRepository;

    public SimulateMatchesRequestHandler(IRepository<Match> matchRepository, IMapper mapper, IGameFormatFactory gameFormatFactory, IRoundService roundService, IRepository<Tournament> tournamentRepository)
    {
        _matchRepository = matchRepository;
        _mapper = mapper;
        _gameFormatFactory = gameFormatFactory;
        _roundService = roundService;
        _tournamentRepository = tournamentRepository;
    }

    //- Simulate all the matches that is made but not played
    public async Task<bool> Handle(SimulateMatchesRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }


        var matches = (await _tournamentRepository.GetAllAsync(t => t.Id == request.TournamentId, [nameof(Tournament.Matches),
                    nameof(Tournament.GameType)]))
                    .SelectMany(t => t.Matches);

        if (matches == null || matches.Count() == 0) throw new NotFoundException("No Matches Found to simulate");

        if(matches.First().Tournament.GameType.Name != Domain.Enum.GameTypeSupported.TableTennis)throw new BadRequestException("can not simulate other than table tennis tournament");

        //get the game type handler
        var gameTypeHandler = _gameFormatFactory.GetGameFormat(matches.First().Tournament.GameType.Name);
        if (gameTypeHandler == null) throw new NotFoundException($"Game type handler could not be resolved");

        foreach (var match in matches)
        {
            //load all the mathces property, this is test will not be in prod
            await _matchRepository.ExplicitLoadReferenceAsync(match, mt => mt.FirstPlayer);
            await _matchRepository.ExplicitLoadReferenceAsync(match, mt => mt.SecondPlayer);
            await _matchRepository.ExplicitLoadReferenceAsync(match, mt => mt.Winner);
            await _matchRepository.ExplicitLoadReferenceAsync(match, mt => mt.Round);

            if (match.Winner != null)
            {
                //match already player
                continue;
            }
            //if any player is bye then winner is the other player
            if(match.FirstPlayer.Name.ToLower().Contains("bye"))
            {
                match.Winner = match.SecondPlayer;
                continue;
            }
            if(match.SecondPlayer.Name.ToLower().Contains("bye"))
            {
                match.Winner = match.FirstPlayer;
                continue;
            }

            var randomlyGeneratedMatchScore = GetRandomScore();

            //use the handler to validate the score
            //IScore gameScore = gameTypeHandler.DeserializeScore((object)randomlyGeneratedMatchScore);
            //if (!gameTypeHandler.IsValidScore(gameScore))
            //{
            //    throw new ValidationException("Invalid game-specific data for the given game type.");
            //}

            //determine the winner
            var winner = gameTypeHandler.DetermineWinner(match.FirstPlayer, match.SecondPlayer, randomlyGeneratedMatchScore);//specify the first player and second player in thhe param, according to gameScore player1 and player2

            //update the match and player stats here
            UpdateMatchAndPlayerStat(winner, gameTypeHandler.SerializeScore(randomlyGeneratedMatchScore), match);

            //if i could send and event and let it complete that would be ideal here.
            //but this is a small app, will not have problem

            //update the round is complete or not
            await _roundService.UpdateRoundCompletion(match.Round);
        }

        await _matchRepository.SaveAsync();
        
        return true;
    }
    private void UpdateMatchAndPlayerStat(Player winner, string serializedScore, Match match)
    {
        //update match 
        match.Winner = winner;
        match.GamePlayed = match.GameScheduled; // if game played is not provided then fall back to tournament scheduled date
        match.ScoreJson = serializedScore;

        //update the game played stat
        match.FirstPlayer.GamePlayed += 1;
        match.SecondPlayer.GamePlayed += 1;

        //update the game won
        if (match.FirstPlayer.Id == winner.Id) match.FirstPlayer.GameWon += 1;
        else match.SecondPlayer.GameWon += 1;

    }

    //making it only for table tennis game format
    private TableTennisScore GetRandomScore()
    {
        var random = new Random();
        var sets = new List<SetScore>();
        int player1Sets = 0;
        int player2Sets = 0;

        for (int i = 0; i < 5; i++)
        {
            int player1Points, player2Points;
            do
            {
                player1Points = random.Next(7, 15);
                player2Points = random.Next(7, 15);
            }
            while (Math.Abs(player1Points - player2Points) < 2);

            sets.Add(new SetScore
            {
                Player1Points = player1Points,
                Player2Points = player2Points
            });

            if (player1Points > player2Points)
            {
                player1Sets++;
            }
            else
            {
                player2Sets++;
            }
        }
        return new TableTennisScore
        {
            Player1Sets = player1Sets,
            Player2Sets = player2Sets,
            SetScores = sets
        };
    }
}
