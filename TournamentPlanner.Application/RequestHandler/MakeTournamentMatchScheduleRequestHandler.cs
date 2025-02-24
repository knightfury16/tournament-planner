using AutoMapper;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Application.Helpers;
using TournamentPlanner.Application.Services;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Enum;
using TournamentPlanner.Domain.Exceptions;
using TournamentPlanner.Mediator;
using MatchType = TournamentPlanner.Domain.Entities.MatchType;

namespace TournamentPlanner.Application;

public class MakeTournamentMatchScheduleRequestHandler : IRequestHandler<MakeTournamentMatchScheduleRequest, IEnumerable<MatchDto>>
{
    private readonly IRepository<Tournament> _tournamentRepository;

    private readonly IMapper _mapper;
    private readonly IDrawService _drawService;
    private readonly IMatchService _matchService;
    private readonly ITournamentService _tournamentService;
    private readonly IMatchScheduler _matchScheduler;

    public MakeTournamentMatchScheduleRequestHandler(IRepository<Tournament> tournamentRepository, IMapper mapper,
                                                    IDrawService drawService, IMatchService matchService, ITournamentService tournamentService, IMatchScheduler matchScheduler)
    {
        _tournamentRepository = tournamentRepository;
        _mapper = mapper;
        _drawService = drawService;
        _matchService = matchService;
        _tournamentService = tournamentService;
        _matchScheduler = matchScheduler;
    }
    public async Task<IEnumerable<MatchDto>?> Handle(MakeTournamentMatchScheduleRequest request, CancellationToken cancellationToken = default)
    {
        if(request == null)throw new ArgumentNullException(nameof(request));

        //get the tournament with all the draw
        var nagivationProperty = Utility.NavigationPrpertyCreator(nameof(Tournament.Draws), nameof(Draw.MatchType), nameof(MatchType.Players));
        var tournament = await _tournamentRepository.GetByIdAsync(request.TournamentId, [nagivationProperty, nameof(Tournament.Matches)]);
        if(tournament == null)throw new NotFoundException(nameof(tournament));
        if(!_tournamentService.AmITheCreator(tournament))throw new AdminOwnershipException();

        if(tournament.Status == TournamentStatus.Completed)throw new BadRequestException("Tournament already completed");

        //go to draw service and see if im able to make schedule
            //ON-TEST: Test this please 29/09/24
        var canISchedule = await _tournamentService.CanISchedule(tournament);
        if(canISchedule == false)throw new BadRequestException("Draws not made or previous draws not complete. Can not make schedule");

        //go to match service with all the draws
        var matches = await _matchService.CreateMatches(tournament, request.SchedulingInfo);

        var scheduledMatches = await _matchScheduler.DefaultMatchScheduler(matches.ToList(), request.SchedulingInfo);

        //ON-TEST 
        //TODO: Recalculate this match count logic. This does not seem right
        //! at worst case will have 20 group with 10 match each, with total 200 matches between them. so cant send all the info. remember it. 

        tournament.Matches.AddRange(scheduledMatches);
         await _tournamentRepository.SaveAsync();
        return _mapper.Map<IEnumerable<MatchDto>>(scheduledMatches);
    }
}
