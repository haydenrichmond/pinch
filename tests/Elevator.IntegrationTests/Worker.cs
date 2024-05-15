using Elevator.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Elevator.IntegrationTests
{
    public class Worker(IServiceScopeFactory scopeFactory) : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var elevatorManager = scope.ServiceProvider.GetRequiredService<IElevatorManager>();

                elevatorManager.Start();


                // Scenario 1
                //Thread.Sleep(1000);
                //await elevatorManager._building.Floors[0].PressUpButton();
                //Thread.Sleep(1000);
                //await elevatorManager._building.Elevators[0].Buttons[5].PressButton();
                //Thread.Sleep(10000);

                //// Scenario 2
                //Thread.Sleep(1000);
                //await elevatorManager._building.Floors[6].PressDownButton();
                //await elevatorManager._building.Elevators[0].Buttons[1].PressButton();
                //await elevatorManager._building.Floors[4].PressDownButton();
                //await elevatorManager._building.Elevators[0].Buttons[1].PressButton();
                //Thread.Sleep(10000);


                //// Scenario 3
                //await elevatorManager._building.Floors[2].PressUpButton();
                //await elevatorManager._building.Floors[4].PressDownButton();
                //await elevatorManager._building.Elevators[0].Buttons[6].PressButton();
                //// Technically this can't be pressed until sufficient time passed that person 2 was picked up in elevator so adding sleep
                //Thread.Sleep(30000); // 30 seconds
                //await elevatorManager._building.Elevators[0].Buttons[0].PressButton();

                // Scenario 4
                await elevatorManager._building.Floors[0].PressUpButton();
                Thread.Sleep(10);
                await elevatorManager._building.Elevators[0].Buttons[5].PressButton();
                Thread.Sleep(10);
                await elevatorManager._building.Floors[4].PressDownButton();
                await elevatorManager._building.Floors[10].PressDownButton();
                Thread.Sleep(10);
                await elevatorManager._building.Elevators[0].Buttons[0].PressButton();
                await elevatorManager._building.Elevators[0].Buttons[0].PressButton();


                Thread.Sleep(100000);

            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}

