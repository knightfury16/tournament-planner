using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Application.Helpers;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Enum;
using TournamentPlanner.Domain.Exceptions;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class CanIMakeDrawRequestHandler : IRequestHandler<CanIMakeDrawRequest, CanIDrawDto>
{

    private readonly IRepository<Tournament> _tournamentRepository;

    private readonly ITournamentService _tournamentService;

    public CanIMakeDrawRequestHandler(IRepository<Tournament> tournamentRepository, ITournamentService tournamentService)
    {
        _tournamentRepository = tournamentRepository;
        _tournamentService = tournamentService;
    }

    public async Task<CanIDrawDto> Handle(CanIMakeDrawRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        //does the tournament exists?
        var navigationProperty = Utility.NavigationPrpertyCreator(nameof(Tournament.Draws), nameof(Draw.MatchType));
        var tournament = await _tournamentRepository.GetByIdAsync(request.TournamentId, [nameof(Tournament.Participants), navigationProperty]);

        if (tournament == null)
        {
            throw new NotFoundException(nameof(tournament), request.TournamentId);
        }

        if (!_tournamentService.AmITheCreator(tournament)) throw new AdminOwnershipException();

        //TODO: Need to check the tournament status
        //only allow if the registration is complete>> if status is completed will not allow to make the draw.
        if (tournament.Status <= TournamentStatus.RegistrationOpen)
        {
            return new CanIDrawDto
            {
                Success = false,
                Message = "Close the tournament Registration to Make Draw"
            };
        }
        //can i make draw? 
        var canIDarw = await _tournamentService.CanIMakeDraw(tournament);

        return new CanIDrawDto
        {
            Success = canIDarw
        };

    }
}
