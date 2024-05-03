using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TournamentPlanner.Application.UseCases.AddPlayer;
using TournamentPlanner.Application.UseCases.PlayerUseCase;

namespace TournamentPlanner.Application
{
    public static class DependencyInjection
    {
        public static void AddApplicationServices (this IServiceCollection services){
            //TODO: Make a aggregate use cases
            services.AddScoped<IPlayerUseCase, PlayerUseCase> ();
        }
        
    }
}