using AutoMapper;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.Helpers;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Enum;
using TournamentPlanner.Domain.Exceptions;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class CreateTournamentDrawRequestHandler : IRequestHandler<CreateTournamentDrawRequest, IEnumerable<DrawDto>>
{
    private readonly IRepository<Tournament> _tournamentRepository;
    private readonly IMapper _mapper;
    private readonly ITournamentService _tournamentService;

    public CreateTournamentDrawRequestHandler(IRepository<Tournament> tournamentRepository, IMapper mapper, ITournamentService tournamentService)
    {
        _tournamentRepository = tournamentRepository;
        _mapper = mapper;
        _tournamentService = tournamentService;
    }


    public async Task<IEnumerable<DrawDto>?> Handle(CreateTournamentDrawRequest request, CancellationToken cancellationToken = default)
    {
        //!! Axiom:: in TP I can only make two draw at most if the type is Group
        //does the tournament exists?
        var navigationProperty = Utility.NavigationPrpertyCreator(nameof(Tournament.Draws), nameof(Draw.MatchType));
        var tournament = (await _tournamentRepository.GetAllAsync(t => t.Id == request.TournamentId, [nameof(Tournament.Participants),
                            navigationProperty]))
                            .FirstOrDefault();

        if (tournament == null)
        {
            throw new NotFoundException(nameof(tournament), request.TournamentId);
        }

        if (!_tournamentService.AmITheCrator(tournament)) throw new BadRequestException("You are not the admin of the tournament");

        //TODO: Need to check the tournament status
        //only allow if the registration is complete>> if status is completed will not allow to make the draw.
        if (tournament.Status <= TournamentStatus.RegistrationOpen)
        {
            throw new BadRequestException("Please close the Tournament Registration in order to make draw");
        }
        //can i make draw? 
        var canIDarw = await _tournamentService.CanIMakeDraw(tournament);

        if (!canIDarw) throw new BadRequestException("Previous draws are not completed.Can not make new draw");

        //make draw -> make matchType
        var draws = await _tournamentService.MakeDraws(tournament, request.MatchTypePrefix, request.SeedersId);
        tournament.Draws.AddRange(draws);
        await _tournamentRepository.SaveAsync();

        return _mapper.Map<IEnumerable<DrawDto>>(draws);
    }
}
