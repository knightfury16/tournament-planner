using System.ComponentModel.DataAnnotations;
using AutoMapper;
using TournamentPlanner.Application.Common;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Enum;
using TournamentPlanner.Mediator;
using MatchType = TournamentPlanner.Domain.Entities.MatchType;

namespace TournamentPlanner.Application;

public class AddMatchInTournamentRequestHandler : IRequestHandler<AddMatchInTournamentRequest, MatchDto>
{
    private readonly IRepository<Tournament> _tournamentRepository;
    private readonly GameTypeFactory gameTypeFactory;
    private readonly Mapper _mapper;
    private readonly IRepository<MatchType> _matchTypeRepository;

    public AddMatchInTournamentRequestHandler(IRepository<Tournament> tournamentRepository, GameTypeFactory gameTypeFactory
    , Mapper mapper, IRepository<MatchType> matchTypeRepository)
    {
        this._matchTypeRepository = matchTypeRepository;
        this._tournamentRepository = tournamentRepository;
        this.gameTypeFactory = gameTypeFactory;
        this._mapper = mapper;
    }
    public async Task<MatchDto?> Handle(AddMatchInTournamentRequest request, CancellationToken cancellationToken = default)
    {
        //does the tournament of the given id exists
        var tournament = (await _tournamentRepository
                         .GetAllAsync(t => t.Id == request.TournamentId, [nameof(Tournament.GameType), nameof(Tournament.Participants)]))
                         .FirstOrDefault();
        if (tournament == null)
        {
            throw new InvalidOperationException("Tournament not found");
        }
        //are the players of the match part of this tournament
        var player1 = tournament.Participants.Find(p => p.Id == request.AddMatchDto.Player1Id);
        var player2 = tournament.Participants.Find(p => p.Id == request.AddMatchDto.Player2Id);
        if (player1 == null || player2 == null)
        {
            throw new InvalidOperationException("One or both player are not registered to this tournament");
        }

        //get the gametype handler from the factory
        var gameType = GetGameTypeSupported(tournament.GameType);
        var gameTypeHandler = gameTypeFactory.GetTheGameTypeHandler(gameType);

        if (gameTypeHandler == null)
        {
            throw new InvalidOperationException("Game type hanlder could not be resolved");
        }
        //use the handler to validate the score
        if (!gameTypeHandler.ValidateGameSpecificData(request.AddMatchDto.GameSpecificScore))
        {
            throw new ValidationException("Invalid game-specific data for the given game type.");
        }
        //add the match to the db
        //TODO: figure out other way for finding match type
        MatchType? matchType = await _matchTypeRepository.GetByIdAsync(1);
        if (matchType == null)
        {
            throw new InvalidOperationException("Could not find match Type");
        }
        var match = new Match
        {
            FirstPlayer = player1,
            SecondPlayer = player2,
            Tournament = tournament,
            MatchType = matchType,
            ScoreJson = request.AddMatchDto.GameSpecificScore,
            GamePlayed = DateTime.UtcNow,
        };

        tournament.Matches.Add(match);
        //save the db context
        await _tournamentRepository.SaveAsync();
        //map the match and return
        return _mapper.Map<MatchDto>(match);
    }

    private GameTypeSupported GetGameTypeSupported(GameType gameType)
    {
        if (Enum.TryParse(gameType.Name, out GameTypeSupported gameTypeSupported))
        {
            return gameTypeSupported;
        }
        else
        {
            throw new InvalidOperationException("Could not parse gametype to enum");
        }
    }
}
