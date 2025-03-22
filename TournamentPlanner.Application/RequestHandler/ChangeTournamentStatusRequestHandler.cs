using TournamentPlanner.Application.Common;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Exceptions;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class ChangeTournamentStatusRequestHandler
    : IRequestHandler<ChangeTournamentStatusRequest, ChangeTournamentStatusResult>
{
    private readonly IRepository<Tournament> _tournamentRepository;
    private readonly ITournamentService _tournamentService;

    public ChangeTournamentStatusRequestHandler(
        IRepository<Tournament> tournamentRepository,
        ITournamentService tournamentService
    )
    {
        _tournamentRepository = tournamentRepository;
        _tournamentService = tournamentService;
    }

    public async Task<ChangeTournamentStatusResult> Handle(
        ChangeTournamentStatusRequest request,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(request);

        var tournament = await _tournamentRepository.GetByIdAsync(request.TournamentId);

        if (tournament == null)
            throw new NotFoundException(nameof(tournament));
        if (!_tournamentService.AmITheCreator(tournament))
            throw new AdminOwnershipException();
        if (tournament.Status == null)
            throw new NotFoundException(nameof(tournament.Status));

        return await _tournamentService.ChangeTournamentStatus(
            tournament,
            request.TournamentStatus
        );
    }
}
