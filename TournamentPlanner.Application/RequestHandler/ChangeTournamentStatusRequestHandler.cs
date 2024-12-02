using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Enum;
using TournamentPlanner.Domain.Exceptions;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class ChangeTournamentStatusRequestHandler : IRequestHandler<ChangeTournamentStatusRequest, bool>
{
    private readonly IRepository<Tournament> _tournamentRepository;
    private readonly ITournamentService _tournamentService;


    public ChangeTournamentStatusRequestHandler(IRepository<Tournament> tournamentRepository, ITournamentService tournamentService)
    {
        _tournamentRepository = tournamentRepository;
        _tournamentService = tournamentService;
    }

    public async Task<bool> Handle(ChangeTournamentStatusRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var tournament = await _tournamentRepository.GetByIdAsync(request.TournamentId);

        if (tournament == null) throw new NotFoundException(nameof(tournament));
        if (!_tournamentService.AmITheCreator(tournament)) throw new AdminOwnershipException();
        if (tournament.Status == null) throw new NotFoundException(nameof(tournament.Status));

        var currentStatus = tournament.Status;
        var requestedChangedStatus = request.TournamentStatus;

        if (CanIChangeStatus(requestedChangedStatus, currentStatus))
        {
            tournament.Status = requestedChangedStatus;
            await _tournamentRepository.SaveAsync();
            return true;
        }

        return false;
    }

    private bool CanIChangeStatus(TournamentStatus requestedChangedStatus, TournamentStatus? currentStatus)
    {
        //I can change between Draft, RegistrationOpen, RegistrationClosed back and forth as much as i want
        //but once status is ongoing i cant go back

        if (currentStatus == TournamentStatus.Completed) return false;

        if (currentStatus >= TournamentStatus.Ongoing && requestedChangedStatus <= TournamentStatus.Ongoing)
        {
            return false;
        }

        return true;
    }
}
