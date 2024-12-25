using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Application;

public class GetAdminCreatedTournamentsRequest : IRequest<List<TournamentDto>>
{

}
