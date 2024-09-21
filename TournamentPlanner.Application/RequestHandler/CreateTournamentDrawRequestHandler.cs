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
        //does the tournament exists?
        var tournament = (await _tournamentRepository.GetAllAsync(t => t.Id == request.TournamentId, [nameof(Tournament.Participants),
                            nameof(Tournament.Draw)]))
                            .FirstOrDefault();

        if (tournament == null)
        {
            throw new NotFoundException(nameof(tournament), request.TournamentId);
        }
        //can i make draw? 
        var canIDarw = await _tournamentService.CanIMakeDraw(tournament);

        if (!canIDarw) throw new BadRequestException("Previous draws are not completed.Can not make new draw");

        //make draw -> make matchType
        var draws = await _tournamentService.MakeDraws(tournament, request.MatchTypePrefix, request.SeedersId);
        tournament.Draw.AddRange(draws);
        await _tournamentRepository.SaveAsync();

        return _mapper.Map<IEnumerable<DrawDto>>(draws);
    }
}
