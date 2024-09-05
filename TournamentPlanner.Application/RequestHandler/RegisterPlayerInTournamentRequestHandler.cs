using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Enum;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class RegisterPlayerInTournamentRequestHandler : IRequestHandler<RegisterPlayerInTournamentRequest, bool>
{
    private readonly IRepository<Tournament> _tournamentRepository;
    private readonly IRepository<Player> _playerRepository;
    public RegisterPlayerInTournamentRequestHandler(IRepository<Tournament> tournamentRepository, IRepository<Player> playerRepository)
    {
        _tournamentRepository = tournamentRepository;
        _playerRepository = playerRepository;
    }
    public async Task<bool> Handle(RegisterPlayerInTournamentRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        //check if tournament exists
        var tournament = (await _tournamentRepository
        .GetAllAsync(t => t.Id == request.RegistrationInTournamentDto.TournamentId, [nameof(Tournament.Participants)]))
        .FirstOrDefault();

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
        var player = await _playerRepository.GetByIdAsync(request.RegistrationInTournamentDto.PlayerId);

        if (player == null)
        {
            throw new InvalidOperationException($"Player not found");
        }

        //check if player age is allowed to participate
        if (tournament.MinimumAgeOfRegistration > 0 && player.Age < tournament.MinimumAgeOfRegistration)
        {
            throw new InvalidOperationException($"Player does not meet the minimum age requirement for {tournament.MinimumAgeOfRegistration}");
        }

        //check if player already registered
        if (tournament.Participants.Any(p => p.Id == player.Id))
        {
            throw new InvalidOperationException("Player is already registered for this tournament");
        }

        //add the player to tournament participant
        tournament.Participants.Add(player);
        await _tournamentRepository.SaveAsync();

        return true;
    }
}
