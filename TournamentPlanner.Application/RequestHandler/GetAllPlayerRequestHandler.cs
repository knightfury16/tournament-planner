using AutoMapper;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Application.Request;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application.RequestHandler
{
    public class GetAllPlayerRequestHandler : IRequestHandler<GetAllPlayerRequest, IEnumerable<PlayerDto>>
    {
        private readonly IRepository<Player> playerRepository;
        private readonly IMapper _mapper;

        public GetAllPlayerRequestHandler(IRepository<Player> playerRepository, IMapper mapper)
        {
            this.playerRepository = playerRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<PlayerDto>?> Handle(GetAllPlayerRequest request, CancellationToken cancellationToken1 = default)
        {
            IEnumerable<Player> players;
            if (request.name != null)
            {
                players = await playerRepository.GetAllAsync(player => player.Name.ToLowerInvariant().Contains(request.name));
            }
            else
            {
                players = await playerRepository.GetAllAsync();
            }

            var playerDtos = _mapper.Map<IEnumerable<PlayerDto>>(players);

            return playerDtos;
        }
    }
}