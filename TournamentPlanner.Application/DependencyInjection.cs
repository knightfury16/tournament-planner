using Microsoft.Extensions.DependencyInjection;
using TournamentPlanner.Mediator;


namespace TournamentPlanner.Application
{
    public static class DependencyInjection
    {
        public static void AddApplicationServices (this IServiceCollection services){
            //TODO: Make a aggregate use cases
            services.AddScoped<IMediator, Mediator.Mediator>();
    
        }
        
    }
}