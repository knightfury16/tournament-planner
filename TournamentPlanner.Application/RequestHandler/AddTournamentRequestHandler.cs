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
    private readonly IRepository<GameType> _gameTypeRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IMapper _mapper;
    public AddTournamentRequestHandler(IMapper mapper, IRepository<Tournament> tournamentRepository, ICurrentUser currentUser, IRepository<Admin> adminRepository, IRepository<GameType> gameTypeRepository)
    {
        _mapper = mapper;
        _tournamentRepository = tournamentRepository;
        _currentUser = currentUser;
        _adminRepository = adminRepository;
        _gameTypeRepository = gameTypeRepository;
    }
    public async Task<TournamentDto?> Handle(AddTournamentRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var admin = await GetAdmin();
        if (admin == null) throw new NotFoundException(nameof(admin));


        var tournament = _mapper.Map<Tournament>(request.AddTournamentDto);
        var gameType = await GetGameTypeFromDb(tournament.GameType);
        tournament.GameType = gameType;
        tournament.CreatedBy = admin;
        var addedTorunament = await _tournamentRepository.AddAsync(tournament);

        await _tournamentRepository.SaveAsync();

        return _mapper.Map<TournamentDto>(addedTorunament);
    }

    private async Task<GameType> GetGameTypeFromDb(GameType tournamentGameType)
    {
        ArgumentNullException.ThrowIfNull(tournamentGameType);
        var gameTypes = await _gameTypeRepository.GetAllAsync(gt => gt.Name == tournamentGameType.Name);
        //ideally there should not be more than one game type
        //but i dont want to remove all the data of my created tournament
        //so keeping it like this
        //TODO: check if there is more than one game type is yes throw error
        var gameType = gameTypes.FirstOrDefault();

        if (gameType == null)throw new ArgumentNullException(nameof(gameType));

        return gameType;
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