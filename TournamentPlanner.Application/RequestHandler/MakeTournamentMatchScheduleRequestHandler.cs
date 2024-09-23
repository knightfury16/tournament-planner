using AutoMapper;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Application.Helpers;
using TournamentPlanner.Application.Services;
using TournamentPlanner.Domain.Entities;
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
        var nagivationProperty = Utility.NavigationPrpertyCreator(nameof(Tournament.Draws), nameof(Draw.MatchType), nameof(MatchType.Players));
        var tournament = await _tournamentRepository.GetByIdAsync(request.TournamentId, [nagivationProperty, nameof(Tournament.Matches)]);
        if(tournament == null)throw new NotFoundException(nameof(tournament));

        //go to draw service and see if im able to make schedule
        var canISchedule = await CanISchedule(tournament);
        if(canISchedule == false)throw new BadRequestException("Previous draws not complete. Can not make schedule");
        //got to match servie with all the draws
        var matches = await _matchService.CreateMatches(tournament, request.SchedulingInfo);

        //ON-TEST 
        //! not saving it to db yet
        //! at worst case will have 20 group with 10 match each, with total 200 matches between them. so cant send all the info. remember it. 
        return _mapper.Map<IEnumerable<MatchDto>>(matches);
    }

    private async Task<bool> CanISchedule(Tournament tourament)
    {
        if (tourament.Matches == null || tourament.Matches.Count() == 0 && tourament.Draws != null) return true; //I have made draws but no mathces yes scheduled or played, initial phase
        if (tourament.Draws == null) return false; //no draws yet created, so can not make schedule
        return await _drawService.IsTheDrawComplete(tourament.Draws); //
    }

}
