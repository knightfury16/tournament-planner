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
    public Task<TournamentDto?> Handle(AddTournamentRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}