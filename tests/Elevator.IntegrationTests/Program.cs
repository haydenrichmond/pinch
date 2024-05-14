using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Elevator.IntegrationTests
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args);
            host.ConfigureAppConfiguration(ConfigureAppConfiguration);
            host.ConfigureServices(ConfigureServices);
            await host.RunConsoleAsync();
        }

        public static void ConfigureAppConfiguration(HostBuilderContext context, IConfigurationBuilder configBuilder)
        {
            configBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            configBuilder.AddUserSecrets(Assembly.GetExecutingAssembly());
            configBuilder.AddEnvironmentVariables();
        }

        public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddElevatorServices(context.Configuration);

            services.AddHostedService<Worker>();
        }
    }
}