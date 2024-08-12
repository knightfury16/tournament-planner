using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application.Request
{
    public class GetAllPlayerRequest : IRequest<IEnumerable<PlayerDto>>
    {
        public string? name { get; set; }

        public GetAllPlayerRequest(string? name)
        {
            this.name = name;
        }
    }
}