using AutoMapper;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Application.Services;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Exceptions;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class MakeTournamentMatchScheduleRequestHandler : IRequestHandler<MakeTournamentMatchScheduleRequest, IEnumerable<MatchDto>>
{
    private readonly IRepository<Tournament> _tournamentRepository;

    private readonly IMapper _mapper;
    private readonly IDrawService _drawService;
    private readonly IMatchService _matchService;

    public MakeTournamentMatchScheduleRequestHandler(IRepository<Tournament> tournamentRepository, IMapper mapper, 
                                                    IDrawService drawService, IMatchService matchService)
    {
        _tournamentRepository = tournamentRepository;
        _mapper = mapper;
        _drawService = drawService;
        _matchService = matchService;
    }
    public async Task<IEnumerable<MatchDto>?> Handle(MakeTournamentMatchScheduleRequest request, CancellationToken cancellationToken = default)
    {
        if(request == null)throw new ArgumentNullException(nameof(request));

        //get the tournament with all the draw
        var nagivationProperty =  nameof(Tournament.Draws) + "." + nameof(Draw.MatchType);
        var tournament = (await _tournamentRepository.GetAllAsync(t => t.Id == request.TournamentId, [nagivationProperty])).FirstOrDefault();
        if(tournament == null)throw new NotFoundException(nameof(tournament));

        //go to draw service and see if im able to make schedule
        var canISchedule = await _drawService.IsTheDrawComplete(tournament.Draws);
        if(canISchedule == false)throw new BadRequestException("Previous draws not complete. Can not make schedule");
        //got to match servie with all the draws
        var matches = await _matchService.CreateMatches(tournament.Draws, GetSchedulingInfo(tournament.StartDate, request));

        return _mapper.Map<IEnumerable<MatchDto>>(matches);
    }

    private SchedulingInfo GetSchedulingInfo(DateTime startDate, MakeTournamentMatchScheduleRequest request)
    {
        return new SchedulingInfo
        {
            StartDate = startDate,
            EachMatchTime = request.EachMatchTime,
            StartTime = request.StartTime,
        };
    }
}
