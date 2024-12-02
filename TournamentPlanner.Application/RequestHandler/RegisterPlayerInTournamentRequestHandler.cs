using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Enum;
using TournamentPlanner.Domain.Exceptions;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class RegisterPlayerInTournamentRequestHandler : IRequestHandler<RegisterPlayerInTournamentRequest, bool>
{
    private readonly IRepository<Tournament> _tournamentRepository;
    private readonly IRepository<Player> _playerRepository;
    private readonly ICurrentUser _currentUser;
    public RegisterPlayerInTournamentRequestHandler(IRepository<Tournament> tournamentRepository, IRepository<Player> playerRepository, ICurrentUser currentUser)
    {
        _tournamentRepository = tournamentRepository;
        _playerRepository = playerRepository;
        _currentUser = currentUser;
    }
    public async Task<bool> Handle(RegisterPlayerInTournamentRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        //check if tournament exists
        var tournament = await _tournamentRepository
        .GetByIdAsync(request.RegistrationInTournamentDto.TournamentId, [nameof(Tournament.Participants)]);

        if (tournament == null)
        {
            throw new InvalidOperationException("Tournament not found");
        }

        //check if the tournament is open for registration
        if (tournament.Status != TournamentStatus.RegistrationOpen)
        {
            throw new InvalidOperationException("Tournament Registration is not open");
        }

        //check if the registration last date is passed or not
        if (tournament.RegistrationLastDate.HasValue && tournament.RegistrationLastDate != default && DateTime.UtcNow > tournament.RegistrationLastDate.Value)
        {
            throw new InvalidOperationException("Registration deadline has passed");
        }

        //check if the paricipant is max or not 
        if (tournament.Participants.Count >= tournament.MaxParticipant)
        {
            throw new InvalidOperationException("Tournament has reached the maximum number of participants");
        }


        //check if the player exist
        var currentPlayerId = GetCurrentUserId();
        if (currentPlayerId == null) throw new NotFoundException("Player cloud not be found");
        var player = await _playerRepository.GetByIdAsync(currentPlayerId.Value);

        if (player == null)
        {
            throw new NotFoundException(nameof(player));
        }

        //check if player age is allowed to participate
        if (tournament.MinimumAgeOfRegistration > 0 && player.Age < tournament.MinimumAgeOfRegistration)
        {
            throw new ValidationException($"Player does not meet the minimum age requirement for {tournament.MinimumAgeOfRegistration}");
        }

        //check if player already registered
        if (tournament.Participants.Any(p => p.Id == player.Id))
        {
            throw new BadRequestException("Player is already registered for this tournament");
        }

        //add the player to tournament participant
        tournament.Participants.Add(player);
        await _tournamentRepository.SaveAsync();

        return true;
    }

    private int? GetCurrentUserId()
    {
        return _currentUser.DomainUserId.HasValue ? _currentUser.DomainUserId.Value : null;
    }
}
