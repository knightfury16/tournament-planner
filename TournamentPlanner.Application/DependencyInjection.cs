using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TournamentPlanner.Application.UseCases.AddPlayer;
using TournamentPlanner.Application.UseCases.GenerateUseCase;
using TournamentPlanner.Application.UseCases.MatchUseCase;
using TournamentPlanner.Application.UseCases.PlayerUseCase;
using TournamentPlanner.Application.UseCases.TournamentUseCase;

namespace TournamentPlanner.Application
{
    public static class DependencyInjection
    {
        public static void AddApplicationServices (this IServiceCollection services){
            //TODO: Make a aggregate use cases
            services.AddScoped<IPlayerUseCase, PlayerUseCase> ();
            services.AddScoped<IMatchUseCase, MatchUseCase> ();
            services.AddScoped<IGenerate, GenerateUseCase> ();
            services.AddScoped<ITournamentUseCase, TournamentUseCase> ();
        }
        
    }
}