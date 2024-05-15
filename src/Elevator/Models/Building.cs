//using Elevator.Configuration;
//using Elevator.Events;
//using Microsoft.Extensions.Options;

//namespace Elevator.Models
//{
//    public class Building
//    {
//        private IOptions<BuildingOptions> _options;
//        public Building(IOptions<BuildingOptions> options)
//        {
//            _options = options;
//            var totalFloorsIncludingGround = options.Value.TotalFloors + 1;
//            Floors = new Floor[totalFloorsIncludingGround];
//            for (var i = 0; i < totalFloorsIncludingGround; i++)
//            {
//                var floor = new Floor(i);
//                floor.CallButtonPressed += async (floorNumber, floor) => await Floor_CallButtonPressed(floorNumber, floor);
//                Floors[i] = floor;
//            }

//            Elevators = new Elevator[_options.Value.TotalElevators];
//            for (var i = 0; i < _options.Value.TotalElevators; i++)
//            {
//                var buttons = new List<Button>();
//                for (var j = 0; j < totalFloorsIncludingGround; j++)
//                {
//                    var button = new Button(j);
//                    button.ElevatorButtonPressed += async (sender, e) => await Button_ElevatorButtonPressed(sender, e);
//                    buttons.Add(button);
//                }

//                var elevator = new Elevator(options, buttons.ToArray()) { IdNumber = i };
//                foreach (var b in buttons)
//                {
//                    b.Elevator = elevator;
//                }
//                elevator.FloorChanged += async (floor) =>
//                {
//                    Console.WriteLine($"Elevator is now at floor {floor}");
//                };

//                elevator.Door = new Door(options);

//                Elevators[i] = elevator;
//            }
//        }

//        public Floor[] Floors { get; set; }

//        public Elevator[] Elevators { get; set; }


//        //private const int FloorStopBuffer = 2;

//        //private async Task Floor_CallButtonPressed(int floorNumber, Floor floor)
//        //{
//        //    Console.WriteLine($"CallButtonPressed for floor: {floorNumber}");


//        //    // TODO select an elevator using more advanced logic.
//        //    // TODO refactor logic into dispatcher service.
//        //    var availableElevator = Elevators.
//        //        OrderByDescending(x => x._moving)
//        //        .ThenBy(x =>
//        //        (x.DirectionOfTravel == DirectionOfTravel.Up && x._currentFloor < floorNumber - FloorStopBuffer)
//        //        ||
//        //        (x.DirectionOfTravel == DirectionOfTravel.Down && x._currentFloor > floorNumber + FloorStopBuffer)
//        //        )
//        //        .ThenBy(x => x.IdNumber)
//        //        .First();

//        //    await availableElevator.QueueFloor(floorNumber, floor.UpButtonPressed);
//        //}

//        private async Task Button_ElevatorButtonPressed(object sender, ElevatorButtonPressedEventArgs e)
//        {
//            if (sender is Button button)
//            {
//                Console.WriteLine($"ElevatorButtonPressed for floor: {button.FloorNumber}");
//                var idleElevator = button.Elevator;
//                await idleElevator.QueueFloor(button.FloorNumber, null);
//            }

//        }
//    }
//}
