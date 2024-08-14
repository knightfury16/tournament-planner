using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace TournamentPlanner.Mediator
{
    public static class ServiceCollectionExtension
    {
        /*TODO: Enhancement feature can be added later
         services.AddMediatorHandlers(options =>
         {
             options.Lifetime = ServiceLifetime.Transient;
             options.TypeFilter = t => t.Namespace.StartsWith("YourNamespace");
             options.Logger = Console.WriteLine;
        }, typeof(AddPlayerCommand).Assembly); 
        */

        public static void AddMediatorHandler(this IServiceCollection services, params Assembly[] assemblies)
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

                    try
                    {
                        services.AddScoped(handlerInterface, handler);
                        Console.WriteLine($"Successfully registered {handler.Name} as {handlerInterface.Name}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error registering {handler.Name} as {handlerInterface.Name}: {ex.Message}");
                        throw; // Re-throw to catch in higher level or to stop the application
                    }
                }

            }
        }
    }
}