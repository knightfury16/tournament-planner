using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace TournamentPlanner.Mediator
{
    public static class ServiceCollectionExtension
    {
        public static void RegisterMediatorHandler(this IServiceCollection services, params Assembly[] assemblies)
        {
            var handlerType = typeof(IRequestHandler<,>);
            foreach (var assembly in assemblies)
            {

                var handlers = assembly.GetTypes().
                    Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerType))
                    .ToList();

                foreach (var handler in handlers)
                {
                    var handlerInterface = handler.GetInterfaces()
                    .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerType);
                    services.AddScoped(handlerInterface, handler);
                }

            }
        }
    }
}