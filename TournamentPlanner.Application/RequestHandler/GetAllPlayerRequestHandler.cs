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
        private readonly IRepository<Player, Player> playerRepository;
        private readonly IMapper _mapper;

        public GetAllPlayerRequestHandler(IRepository<Player, Player> playerRepository, IMapper mapper)
        {
            this.playerRepository = playerRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<PlayerDto>?> Handle(GetAllPlayerRequest request, CancellationToken cancellationToken1 = default)
        {
            IEnumerable<Player> players;
            if (request.Name != null)
            {
                players = await playerRepository.GetAllAsync(player => player.Name.ToLowerInvariant().Contains(request.Name));
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