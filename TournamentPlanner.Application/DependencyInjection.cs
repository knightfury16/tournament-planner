using Microsoft.Extensions.DependencyInjection;
using TournamentPlanner.Application.Request;
using TournamentPlanner.Application.RequestHandler;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Mediator;


namespace TournamentPlanner.Application
{
    public static class DependencyInjection
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            //TODO: Make a aggregate use cases
            services.AddScoped<IMediator, Mediator.Mediator>();
            services.AddScoped<IRequestHandler<GetAllPlayerRequest, IEnumerable<Player>>, GetAllPlayerRequestHandler>();

        }

    }
}