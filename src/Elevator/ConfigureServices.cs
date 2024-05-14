using Elevator.Configuration;
using Elevator.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Elevator
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddElevatorServices(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<BuildingOptions>(config.GetSection(BuildingOptions.SectionName));

            services.AddScoped<IElevatorManager, ElevatorManager>();
            services.AddScoped<IElevatorDispatcherService, ElevatorDispatcherService>();

            return services;
        }
    }
}
