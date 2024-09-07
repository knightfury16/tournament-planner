using AutoMapper;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Application.Request;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application.RequestHandler
{
    public class GetTournamentByIdRequestHandler: IRequestHandler<GetTournamentByIdRequest, TournamentDto>
    {

        private readonly IRepository<Tournament> _tournamentRepository;
        private readonly IMapper _mapper;

        public GetTournamentByIdRequestHandler(IRepository<Tournament> tournamentRepository, IMapper mapper)
        {
            _tournamentRepository = tournamentRepository;
            _mapper = mapper;
        }
        public async Task<TournamentDto?> Handle(GetTournamentByIdRequest request, CancellationToken cancellationToken = default)
        {
            if(request == null){
                throw new ArgumentNullException(nameof(GetTournamentByIdRequest));
            }

            var tournament = await _tournamentRepository.GetAllAsync(t => t.Id == request.Id,[nameof(Tournament.GameType)]);

            if (tournament == null || !tournament.Any()) return null;

            return _mapper.Map<TournamentDto>(tournament.First());

        }

    }
}