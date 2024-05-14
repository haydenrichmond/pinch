using Elevator.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Elevator.IntegrationTests
{
    public class Worker(IServiceScopeFactory scopeFactory) : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var elevatorManager = scope.ServiceProvider.GetRequiredService<IElevatorManager>();

                elevatorManager.Start();

                Thread.Sleep(1000);

                elevatorManager._building.Floors[0].PressUpButton();


                Thread.Sleep(1000);

                elevatorManager._building.Elevators[0].Buttons[5].PressButton();

                Thread.Sleep(10000);
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}

