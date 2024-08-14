using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application.Request
{
    public class AddPlayerRequest : IRequest<PlayerDto>
    {
        public AddPlayerDto AddPlayerDto { get; set; }
        public AddPlayerRequest(AddPlayerDto addPlayerDto)
        {
            AddPlayerDto = addPlayerDto;
        }

    }
}