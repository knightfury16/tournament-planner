using AutoMapper;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class GetAdminCreatedTournamentsRequestHandler : IRequestHandler<GetAdminCreatedTournamentsRequest, List<TournamentDto>>
{

    private readonly IRepository<Tournament> _tournamentRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IMapper _mapper;

    public GetAdminCreatedTournamentsRequestHandler(IRepository<Tournament> tournamentRepository, ICurrentUser currentUser, IMapper mapper)
    {
        _tournamentRepository = tournamentRepository;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<List<TournamentDto>?> Handle(GetAdminCreatedTournamentsRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var adminId = _currentUser.DomainUserId;
        if (adminId == null)
        {
            throw new ArgumentNullException(nameof(adminId), "AdminId cannot be null.");
        }

        var tournaments = await _tournamentRepository.GetAllAsync(t => t.CreatedBy.Id == adminId, [nameof(Tournament.GameType)]);

        return _mapper.Map<List<TournamentDto>>(tournaments);
    }
}
