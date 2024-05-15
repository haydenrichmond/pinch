using Elevator.Configuration;
using Elevator.Events;
using Elevator.Models;
using Microsoft.Extensions.Options;

namespace Elevator.Services
{
    public interface IElevatorManager
    {
        void Start();
        void Stop();

        public Floor[] Floors { get; }
        public Models.Elevator[] Elevators { get; }
    }
    public class ElevatorManager : IElevatorManager
    {
        private readonly IElevatorDispatcherService _elevatorDispatcherService;
        private readonly IOptions<BuildingOptions> _options;

        public Floor[] Floors { get; set; }

        public Models.Elevator[] Elevators { get; set; }

        public ElevatorManager(IOptions<BuildingOptions> config, IElevatorDispatcherService elevatorDispatcherService)
        {
            _options = config;
            _elevatorDispatcherService = elevatorDispatcherService;
        }


        public void Start()
        {
            var totalFloorsIncludingGround = _options.Value.TotalFloors + 1;
            Floors = new Floor[totalFloorsIncludingGround];
            for (var f = 0; f < totalFloorsIncludingGround; f++)
            {
                var floor = new Floor(f);
                floor.CallButtonPressed += async (floorNumber, floor) => await Floor_CallButtonPressed(floorNumber, floor);
                Floors[f] = floor;
            }

            Elevators = new Models.Elevator[_options.Value.TotalElevators];
            for (var e = 0; e < _options.Value.TotalElevators; e++)
            {
                var buttons = new List<Button>();
                for (var b = 0; b < totalFloorsIncludingGround; b++)
                {
                    var button = new Button(b);
                    button.ElevatorButtonPressed += async (sender, e) => await Button_ElevatorButtonPressed(sender, e);
                    buttons.Add(button);
                }

                var elevator = new Models.Elevator(_options, buttons.ToArray()) { IdNumber = e };
                foreach (var b in buttons)
                {
                    b.Elevator = elevator;
                }
                elevator.FloorChanged += async (floor) =>
                {
                    Console.WriteLine($"Elevator is now at floor {floor}");
                };

                elevator.Door = new Door(_options);

                Elevators[e] = elevator;
            }
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }


        private async Task Floor_CallButtonPressed(int floorNumber, Floor floor)
        {
            Console.WriteLine($"CallButtonPressed for floor: {floorNumber}");
            await _elevatorDispatcherService.Dispatch(Elevators, floorNumber, floor.UpButtonPressed);
        }

        private async Task Button_ElevatorButtonPressed(object sender, ElevatorButtonPressedEventArgs e)
        {
            if (sender is Button button)
            {
                Console.WriteLine($"ElevatorButtonPressed for floor: {button.FloorNumber}");
                var idleElevator = button.Elevator;
                await idleElevator.QueueFloor(button.FloorNumber, null);
            }

        }
    }
}
