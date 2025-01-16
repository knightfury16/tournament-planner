using System.Linq.Expressions;
using AutoMapper;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Application.Enums;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class GetTournamentRequestHandler : IRequestHandler<GetTournamentRequest, IEnumerable<TournamentDto>>
{
    private readonly IRepository<Tournament> _tournamentRepository;
    private readonly IMapper _mapper;

    public GetTournamentRequestHandler(IRepository<Tournament> tournamentRepository, IMapper mapper)
    {
        _tournamentRepository = tournamentRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TournamentDto>?> Handle(GetTournamentRequest request, CancellationToken cancellationToken = default)
    {
        var filters = new List<Expression<Func<Tournament, bool>>>();

        //Name filter
        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            filters.Add(t => t.Name.ToLower().Contains(request.Name));
        }

        //Status filter
        if (request.Status.HasValue)
        {
            filters.Add(t => t.Status == request.Status);
        }

        //GameType filter
        if (request.GameTypeSupported.HasValue)
        {
            filters.Add(t => t.GameType.Name == request.GameTypeSupported);
        }

        //Date range filter
        if (request.StartDate.HasValue || request.EndDate.HasValue)
        {
            filters.Add(GetDateRangeFilter(request.StartDate, request.EndDate));
        }
        else
        {
            //default date range
            // filters.Add(GetSearchCategoryFilter(request.SearchCategory));
        }

        var tournaments = await _tournamentRepository.GetAllAsync(filters,[nameof(Tournament.GameType)]);

        return _mapper.Map<IEnumerable<TournamentDto>>(tournaments);

    }

    private Expression<Func<Tournament, bool>> GetSearchCategoryFilter(TournamentSearchCategory searchCategory)
    {
        var today = DateTime.UtcNow.Date;
        var weekStart = today.AddDays(-(int)today.DayOfWeek);
        var weekEnd = weekStart.AddDays(7);

        return searchCategory switch
        {
            TournamentSearchCategory.Recent => t => t.EndDate <= today && t.EndDate >= today.AddDays(-7),
            TournamentSearchCategory.ThisWeek => t => t.StartDate >= weekStart && t.StartDate < weekEnd,
            TournamentSearchCategory.Upcoming => t => t.StartDate > today,
            TournamentSearchCategory.All => t => true,
            _ => t => t.StartDate >= weekStart && t.StartDate < weekEnd,
        };
    }

    private Expression<Func<Tournament, bool>> GetDateRangeFilter(DateTime? startDate, DateTime? endDate)
    {
        return t => (!startDate.HasValue || t.StartDate >= startDate.Value) &&
                    (!endDate.HasValue || t.EndDate <= endDate.Value);
    }
}