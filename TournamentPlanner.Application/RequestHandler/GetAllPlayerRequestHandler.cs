using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.Request;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application.RequestHandler
{
    public class GetAllPlayerRequestHandler : IRequestHandler<GetAllPlayerRequest, IEnumerable<Player>>
    {
        private readonly IRepository<Player, Player> playerRepository;

        public GetAllPlayerRequestHandler(IRepository<Player, Player> playerRepository)
        {
            this.playerRepository = playerRepository;
        }
        public async Task<IEnumerable<Player>?> Handle(GetAllPlayerRequest request, CancellationToken cancellationToken1 = default)
        {
            if(request.name != null){
                return await playerRepository.GetAllAsync(player => player.Name.ToLowerInvariant().Contains(request.name));
            }

            var players = await playerRepository.GetAllAsync();
            return players;
        }
    }
}