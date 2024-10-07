using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TournamentPlanner.Application.Common;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Application.Services;
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
            services.AddScoped<IGameFormatFactory, GameFormatFactory>();
            services.AddScoped<ICreateMatchTypeFactory, CreateMatchTypeFactory>();
            services.AddScoped<ITournamentService, TournamentService>();
            services.AddScoped<IDrawService, DrawService>();
            services.AddScoped<IMatchTypeService, MatchTypeService>();
            services.AddScoped<IMatchService, MatchService>();
            services.AddScoped<IMatchScheduler, MatchScheduler>();
            services.AddScoped<IRoundService, RoundService>();
            services.AddScoped<IRoundRobin, CreateRoundRobinMatches>();
            services.AddScoped<IKnockout, CreateKnockOutMatches>();



        }

    }
}