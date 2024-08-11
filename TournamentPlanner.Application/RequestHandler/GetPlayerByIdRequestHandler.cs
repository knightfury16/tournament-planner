using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.Request;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application.RequestHandler
{
    public class GetPlayerByIdRequestHandler : IRequestHandler<GetPlayerByIdRequest, Player>
    {
        private readonly IRepository<Player, Player> _playerRepository;

        public GetPlayerByIdRequestHandler(IRepository<Player, Player> playerRepository)
        {
            _playerRepository = playerRepository;
        }
        public async Task<Player?> Handle(GetPlayerByIdRequest request, CancellationToken cancellationToken1 = default)
        {

            //TODO: include the matched property of the palyer
            var player = await _playerRepository.GetByIdAsync(request.id);

            if (player == null) return null;

            return player;

        }

    }
}