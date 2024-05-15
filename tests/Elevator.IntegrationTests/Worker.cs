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
                cancellationToken.Register(elevatorManager.Stop);


                //Console.WriteLine("*********SCENARIO 1*********");
                //Task.Delay(1000);
                //await elevatorManager.Floors[0].PressUpButton();
                //Task.Delay(1000);
                //await elevatorManager.Elevators[0].Buttons[5].PressButton();
                //Task.Delay(10000);

                //Console.WriteLine("*********SCENARIO 2*********");
                //Task.Delay(1000);
                //await elevatorManager.Floors[6].PressDownButton();
                //await elevatorManager.Elevators[0].Buttons[1].PressButton();
                //await elevatorManager.Floors[4].PressDownButton();
                //await elevatorManager.Elevators[0].Buttons[1].PressButton();
                //Task.Delay(10000);


                //Console.WriteLine("*********SCENARIO 3*********");
                //await elevatorManager.Floors[2].PressUpButton();
                //await elevatorManager.Floors[4].PressDownButton();
                //await elevatorManager.Elevators[0].Buttons[6].PressButton();
                //// Technically this can't be pressed until sufficient time passed that person 2 was picked up in elevator so adding sleep
                //Task.Delay(30000); // 30 seconds
                //await elevatorManager.Elevators[0].Buttons[0].PressButton();


                Console.WriteLine("*********SCENARIO 4*********");
                await elevatorManager.Floors[0].PressUpButton();
                Task.Delay(10);
                await elevatorManager.Elevators[0].Buttons[5].PressButton();
                Task.Delay(10);
                await elevatorManager.Floors[4].PressDownButton();
                await elevatorManager.Floors[10].PressDownButton();
                Task.Delay(10);
                await elevatorManager.Elevators[0].Buttons[0].PressButton();
                await elevatorManager.Elevators[0].Buttons[0].PressButton();


                Task.Delay(100000);

            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}

