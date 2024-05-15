using Elevator.Configuration;
using Elevator.Events;
using Microsoft.Extensions.Options;

namespace Elevator.Models
{
    public class Building
    {
        private IOptions<BuildingOptions> _options;
        public Building(IOptions<BuildingOptions> config)
        {
            _options = config;
            var totalFloorsIncludingGround = config.Value.TotalFloors + 1;
            Floors = new Floor[totalFloorsIncludingGround];
            for (var i = 0; i < totalFloorsIncludingGround; i++)
            {
                var floor = new Floor(i);
                floor.CallButtonPressed += async (floorNumber, floor) => await Floor_CallButtonPressed(floorNumber, floor);
                Floors[i] = floor;
            }

            Elevators = new Elevator[_options.Value.TotalElevators];
            for (var i = 0; i < _options.Value.TotalElevators; i++)
            {
                var buttons = new List<Button>();
                for (var j = 0; j < totalFloorsIncludingGround; j++)
                {
                    var button = new Button(j);
                    button.ElevatorButtonPressed += async (sender, e) => await Button_ElevatorButtonPressed(sender, e);
                    buttons.Add(button);
                }

                var elevator = new Elevator(config, buttons.ToArray()) { IdNumber = i };
                foreach (var b in buttons)
                {
                    b.Elevator = elevator;
                }
                elevator.FloorChanged += async (floor) =>
                {
                    Console.WriteLine($"Elevator is now at floor {floor}");
                };

                //elevator.ArrivedAtFloor += async (floor) => {
                //    Console.WriteLine($"Arrived at floor {floor}. Trigger opening doors");
                //    await elevator.Door.Open();
                //    Console.WriteLine("Passengers boarding");
                //    await Task.Delay(1000);
                //    await elevator.Door.Close();
                //    Console.WriteLine("GO!");
                //};

                elevator.Door = new Door(config);

                Elevators[i] = elevator;
            }
        }

        public Floor[] Floors { get; set; }

        public Elevator[] Elevators { get; set; }


        private const int FloorStopBuffer = 2;

        private async Task Floor_CallButtonPressed(int floorNumber, Floor floor)
        {
            Console.WriteLine($"CallButtonPressed for floor: {floorNumber}");


            // TODO select an elevator using more advanced logic.
            // TODO refactor logic into dispatcher service.
            var availableElevator = Elevators.
                OrderByDescending(x => x._moving)
                .ThenBy(x =>
                (x.DirectionOfTravel == DirectionOfTravel.Up && x._currentFloor < floorNumber - FloorStopBuffer)
                ||
                (x.DirectionOfTravel == DirectionOfTravel.Down && x._currentFloor > floorNumber + FloorStopBuffer)
                )
                .ThenBy(x => x.IdNumber)
                .First();


            //Elevators ordered by
            //1) idle
            //2) moving in the direction of the floor
            //3 whatever left over
            await availableElevator.QueueFloor(floorNumber, floor.UpButtonPressed);
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

        //public async Task MoveToTargetFloor(object sender, MoveToTargetFloorEventArgs e)
        //{
        //    if (e.TargetFloor > e.Elevator.MaxFloor || e.TargetFloor < e.Elevator.MinFloor)
        //    {
        //        throw new ArgumentOutOfRangeException(nameof(e.TargetFloor), e.TargetFloor.ToString());
        //    }


        //    if (e.Elevator._currentFloor == e.TargetFloor)
        //    {
        //        return;
        //    }

        //   // e.Elevator.TargetFloor = e.TargetFloor;
        //    //e.Elevator.Status = ElevatorStatus.Moving;
        //    var moveUp = e.Elevator._currentFloor < e.TargetFloor;

        //    if (moveUp)
        //    {
        //        Console.WriteLine($"Elevator moving up to floor {e.TargetFloor}.");
        //        e.Elevator.DirectionOfTravel = Models.DirectionOfTravel.Up;

        //        while (e.Elevator._currentFloor < e.TargetFloor)
        //        {
        //            Console.WriteLine($"Elevator on floor {e.Elevator._currentFloor}.");
        //            var autoEvent = new AutoResetEvent(false);
        //            //var timer = new Timer(e.Elevator.MoveToTargetFloorMethod(e.TargetFloor), autoEvent, 1000, 250);
        //            Thread.Sleep(_options.Value.TimeToTravelBetweenFloorsMilliseconds);
        //            e.Elevator._currentFloor++;
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine($"Elevator moving down to floor {e.TargetFloor}.");
        //        e.Elevator.DirectionOfTravel = Models.DirectionOfTravel.Down;
        //        while (e.Elevator._currentFloor > e.TargetFloor)
        //        {
        //            Console.WriteLine($"Elevator on floor {e.Elevator._currentFloor}.");
        //            Thread.Sleep(_options.Value.TimeToTravelBetweenFloorsMilliseconds);
        //            e.Elevator._currentFloor--;
        //        }
        //    }

        //    // Refactor to reset elevator method
        //    e.Elevator.Buttons[e.TargetFloor].ButtonActive = false;
        //    e.Elevator._currentFloor = e.TargetFloor;
        //    //e.Elevator.TargetFloor = null;
        //    //e.Elevator.Status = ElevatorStatus.Idle;
        //    e.Elevator.DirectionOfTravel = null;
        //    Console.WriteLine($"Elevator on floor {e.Elevator._currentFloor}.");
        //}
    }
}
