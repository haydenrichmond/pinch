using Elevator.Configuration;
using Elevator.Events;
using Microsoft.Extensions.Options;

namespace Elevator.Models
{
    public class Building
    {
        public Building(IOptions<BuildingOptions> config)
        {
            Floors = new Floor[config.Value.TotalFloors];
            for (var i = 0; i < config.Value.TotalFloors; i++)
            {
                var floor = new Floor(i);
                floor.CallButtonPressed += Floor_CallButtonPressed;
                Floors[i] = floor;
            }

            Elevators = new Elevator[config.Value.TotalElevators];
            for (var i = 0; i < config.Value.TotalElevators; i++)
            {
                var buttons = new List<Button>();
                for (var j = 0; j < config.Value.TotalFloors; j++)
                {
                    var button = new Button(j);
                    button.ElevatorButtonPressed += Button_ElevatorButtonPressed;
                    buttons.Add(button);
                }

                var elevator = new Elevator(config, buttons.ToArray()) { Status = ElevatorStatus.Idle };
                foreach (var b in buttons)
                {
                    b.Elevator = elevator;
                }


                Elevators[i] = elevator;
            }
        }

        public Floor[] Floors { get; set; }

        public Elevator[] Elevators { get; set; }


        private const int FloorStopBuffer = 2;

        private void Floor_CallButtonPressed(object sender, CallButtonPressedEventArgs e)
        {
            Console.WriteLine($"CallButtonPressed for floor: {e.Floor.FloorNumber}");

            // TODO select an elevator using more advanced logic.
            // TODO refactor logic into dispatcher service.
            var availableElevator = Elevators.FirstOrDefault(x => x.Status == ElevatorStatus.Idle
            ||
            (x.DirectionOfTravel == DirectionOfTravel.Up && x.CurrentFloor < e.Floor.FloorNumber - FloorStopBuffer)
            ||
            (x.DirectionOfTravel == DirectionOfTravel.Down && x.CurrentFloor > e.Floor.FloorNumber + FloorStopBuffer)
            );

            if (availableElevator == null)
            {
                Console.WriteLine("No elevators available to serve at this time.");
                //TODO handle this scenario in a queu of some sort.
            }

            availableElevator.MoveToTargetFloor(e.Floor.FloorNumber);
        }

        private void Button_ElevatorButtonPressed(object sender, ElevatorButtonPressedEventArgs e)
        {
            Console.WriteLine($"ElevatorButtonPressed for floor: {e.Button.FloorNumber}");

            // TODO select an elevator using more advanced logic.
            // TODO refactor logic into dispatcher service.
            var idleElevator = e.Button.Elevator;
            idleElevator.MoveToTargetFloor(e.Button.FloorNumber);
        }
    }
}
