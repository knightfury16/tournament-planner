using AutoMapper;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Application.Request;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application.RequestHandler
{
    public class GetPlayerByIdRequestHandler : IRequestHandler<GetPlayerByIdRequest, FullPlayerDto>
    {
        private readonly IRepository<Player> _playerRepository;
        private readonly IMapper _mapper;

        public GetPlayerByIdRequestHandler(IRepository<Player> playerRepository, IMapper mapper)
        {
            _playerRepository = playerRepository;
            _mapper = mapper;
        }
        public async Task<FullPlayerDto?> Handle(GetPlayerByIdRequest request, CancellationToken cancellationToken = default)
        {

            var player = await _playerRepository.GetAllAsync(player => player.Id == request.id, [nameof(Player.Tournaments)]);

            if (player == null || !player.Any()) return null;

            return _mapper.Map<FullPlayerDto>(player.First());

        }

    }
}