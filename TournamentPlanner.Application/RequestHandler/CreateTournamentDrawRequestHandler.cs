using AutoMapper;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Domain.Entities;
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
        //else only one that is knockout
        //does the tournament exists?
        var tournament = (await _tournamentRepository.GetAllAsync(t => t.Id == request.TournamentId, [nameof(Tournament.Participants),
                            nameof(Tournament.Draws)]))
                            .FirstOrDefault();

        if (tournament == null)
        {
            throw new NotFoundException(nameof(tournament), request.TournamentId);
        }
        //TODO: Need to check the tournament status
        //only allow if the registration is complete>> if status is completed will not allow to make the draw.
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
