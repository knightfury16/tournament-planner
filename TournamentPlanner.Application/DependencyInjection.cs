using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TournamentPlanner.Application.Common;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Mediator;


namespace TournamentPlanner.Application
{
    public static class DependencyInjection
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            services.AddScoped<IMediator, Mediator.Mediator>();
            services.AddAutoMapper(executingAssembly);
            //order is importent here. Register after adding mediator
            services.AddMediatorHandler([executingAssembly]);
            services.AddScoped<IGameTypeFactory, GameTypeFactory>();

        }

    }
}