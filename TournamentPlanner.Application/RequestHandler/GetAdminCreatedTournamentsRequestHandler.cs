using System.Linq.Expressions;
using AutoMapper;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Application.Enums;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class GetAdminCreatedTournamentsRequestHandler
    : IRequestHandler<GetAdminCreatedTournamentsRequest, List<TournamentDto>>
{
    private readonly IRepository<Tournament> _tournamentRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IMapper _mapper;

    public GetAdminCreatedTournamentsRequestHandler(
        IRepository<Tournament> tournamentRepository,
        ICurrentUser currentUser,
        IMapper mapper
    )
    {
        _tournamentRepository = tournamentRepository;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<List<TournamentDto>?> Handle(
        GetAdminCreatedTournamentsRequest request,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(request);

        var adminId = _currentUser.DomainUserId;
        if (adminId == null)
        {
            throw new ArgumentNullException(nameof(adminId), "AdminId cannot be null.");
        }

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
            filters.Add(GetSearchCategoryFilter(request.SearchCategory));
        }

        filters.Add(t => t.CreatedBy.Id == adminId);

        var tournaments = await _tournamentRepository.GetAllAsync(
            filters,
            [nameof(Tournament.GameType)]
        );

        return _mapper.Map<List<TournamentDto>>(tournaments);
    }

    private Expression<Func<Tournament, bool>> GetSearchCategoryFilter(
        TournamentSearchCategory searchCategory
    )
    {
        var today = DateTime.UtcNow.Date;
        var weekStart = today.AddDays(-(int)today.DayOfWeek);
        var weekEnd = weekStart.AddDays(7);

        return searchCategory switch
        {
            TournamentSearchCategory.Recent => t =>
                t.EndDate <= today && t.EndDate >= today.AddDays(-7),
            TournamentSearchCategory.ThisWeek => t =>
                t.StartDate >= weekStart && t.StartDate < weekEnd,
            TournamentSearchCategory.Upcoming => t => t.StartDate > today,
            TournamentSearchCategory.All => t => true,
            _ => t => t.StartDate >= weekStart && t.StartDate < weekEnd,
        };
    }

    private Expression<Func<Tournament, bool>> GetDateRangeFilter(
        DateTime? startDate,
        DateTime? endDate
    )
    {
        return t =>
            (!startDate.HasValue || t.StartDate >= startDate.Value)
            && (!endDate.HasValue || t.EndDate <= endDate.Value);
    }
}
