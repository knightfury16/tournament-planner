using AutoMapper;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Application.Request;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Exceptions;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application.RequestHandler;
public class AddTournamentRequestHandler : IRequestHandler<AddTournamentRequest, TournamentDto>
{
    private readonly IRepository<Tournament> _tournamentRepository;
    private readonly IRepository<Admin> _adminRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IMapper _mapper;
    public AddTournamentRequestHandler(IMapper mapper, IRepository<Tournament> tournamentRepository, ICurrentUser currentUser, IRepository<Admin> adminRepository)
    {
        _mapper = mapper;
        _tournamentRepository = tournamentRepository;
        _currentUser = currentUser;
        _adminRepository = adminRepository;
    }
    public async Task<TournamentDto?> Handle(AddTournamentRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var admin = await GetAdmin();
        if (admin == null) throw new NotFoundException(nameof(admin));


        var tournament = _mapper.Map<Tournament>(request.AddTournamentDto);
        tournament.CreatedBy = admin;
        var addedTorunament = await _tournamentRepository.AddAsync(tournament);

        await _tournamentRepository.SaveAsync();

        return _mapper.Map<TournamentDto>(addedTorunament);
    }

    private async Task<Admin?> GetAdmin()
    {
        var domainUserId = _currentUser.DomainUserId ?? 0;

        Admin? admin = null;

        if (domainUserId > 0)
        {
            admin = await _adminRepository.GetByIdAsync(domainUserId);
        }

        var email = _currentUser.Email;

        if (email != null)
        {
            admin = (await _adminRepository.GetAllAsync(ad => ad.Email == email)).FirstOrDefault();
        }

        return admin;
    }
}