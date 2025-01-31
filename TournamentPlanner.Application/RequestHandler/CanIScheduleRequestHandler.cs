using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.Helpers;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class CanIScheduleRequestHandler : IRequestHandler<CanIScheduleRequest, CanIScheduleDto>
{
    private readonly IRepository<Tournament> _tournamentRepository;
    private readonly ITournamentService _tournamentService;

    public CanIScheduleRequestHandler(IRepository<Tournament> tournamentRepository, ITournamentService tournamentService)
    {
        _tournamentRepository = tournamentRepository;
        _tournamentService = tournamentService;
    }

    public async Task<CanIScheduleDto?> Handle(CanIScheduleRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var navigationProperty = Utility.NavigationPrpertyCreator(nameof(Tournament.Draws), nameof(Draw.MatchType));
        var tournament = await _tournamentRepository.GetByIdAsync(request.TournamentId, [navigationProperty]);
        if (tournament == null) return new CanIScheduleDto { Success = false, Message = "Toutnament with the given Id not found" };
        try
        {
            var canISchedule = await _tournamentService.CanISchedule(tournament);
            return new CanIScheduleDto { Success = canISchedule };
        }
        catch (System.Exception ex)
        {
            return new CanIScheduleDto { Success = false, Message = ex.Message };
        }
    }
}
