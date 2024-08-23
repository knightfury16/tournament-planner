using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application.Request
{
    public class GetAllPlayerRequest : IRequest<IEnumerable<PlayerDto>>
    {
        private string? _name;
        public string? Name { 
            get {return _name;}
            set { _name = value?.ToLower(); }
         }

        public GetAllPlayerRequest(string? name)
        {
            Name = name;
        }

        public GetAllPlayerRequest()
        {
        }
    }
}