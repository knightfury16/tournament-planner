using AutoMapper;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Application.Request;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application.RequestHandler;
public class AddTournamentRequestHandler : IRequestHandler<AddTournamentRequest, TournamentDto>
{
    private readonly IRepository<Tournament> _tournamentRepository;
    private readonly IMapper _mapper;
    public AddTournamentRequestHandler(IMapper mapper, IRepository<Tournament> tournamentRepository)
    {
        _mapper = mapper;
        _tournamentRepository = tournamentRepository;
    }
    public async Task<TournamentDto?> Handle(AddTournamentRequest request, CancellationToken cancellationToken = default)
    {
        //TODO: Only Admin create Tournament, so dynamically find Admin from token
        Admin testAdmin = new Admin() //Id = 4
        {
            Name = "Test creator",
            Email = "test@gmail.com",
            PhoneNumber = "12345"
        };

        var tournament = _mapper.Map<Tournament>(request.AddTournamentDto);
        tournament.AdminId = 4;
        var addedTorunament = await _tournamentRepository.AddAsync(tournament);

        await _tournamentRepository.SaveAsync();

        return _mapper.Map<TournamentDto>(addedTorunament);
    }
}