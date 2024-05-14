using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Elevator.IntegrationTests
{
    public class Worker(IServiceScopeFactory scopeFactory) : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}

