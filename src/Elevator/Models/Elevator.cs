using Elevator.Configuration;
using Microsoft.Extensions.Options;

namespace Elevator.Models
{
    public class Elevator
    {
        private readonly BuildingOptions _config;
        public Elevator(IOptions<BuildingOptions> config, Button[] buttons)
        {
            _config = config.Value ?? throw new Exception($"Missing {nameof(BuildingOptions)}");
            Buttons = buttons;
        }


        public ElevatorStatus Status { get; set; }

        public Door Door { get; set; }

        public Button[] Buttons { get; set; }

        public int MaxFloor => _config.TotalFloors;
        public int MinFloor => 0;

        /// <summary>
        /// 0 denotes floor G
        /// </summary>
        public int CurrentFloor { get; set; }

        public int? TargetFloor { get; set; }


        public DirectionOfTravel? DirectionOfTravel { get; set; }


        public decimal CurrentSpeed { get; set; }


        public void MoveToTargetFloor(int targetFloor)
        {
            if (targetFloor > MaxFloor || targetFloor < MinFloor)
            {
                throw new ArgumentOutOfRangeException(nameof(targetFloor), targetFloor.ToString());
            }


            if (CurrentFloor == targetFloor)
            {
                return;
            }

            TargetFloor = targetFloor;
            Status = ElevatorStatus.Moving;
            var moveUp = CurrentFloor < targetFloor;

            if (moveUp)
            {
                Console.WriteLine($"Elevator moving up to floor {targetFloor}.");
                DirectionOfTravel = Models.DirectionOfTravel.Up;

                while (CurrentFloor < targetFloor)
                {
                    Console.WriteLine($"Elevator on floor {CurrentFloor}.");
                    Thread.Sleep(_config.TimeToTravelBetweenFloorsMilliseconds);
                    CurrentFloor++;
                }
            }
            else
            {
                Console.WriteLine($"Elevator moving down to floor {targetFloor}.");
                DirectionOfTravel = Models.DirectionOfTravel.Down;
                while (CurrentFloor > targetFloor)
                {
                    Console.WriteLine($"Elevator on floor {CurrentFloor}.");
                    Thread.Sleep(_config.TimeToTravelBetweenFloorsMilliseconds);
                    CurrentFloor--;
                }
            }

            CurrentFloor = targetFloor;
            TargetFloor = null;
            Status = ElevatorStatus.Idle;
            DirectionOfTravel = null;
            Console.WriteLine($"Elevator on floor {CurrentFloor}.");
        }

    }

    public enum ElevatorStatus
    {
        Moving,
        Idle
    }

    public enum DirectionOfTravel
    {
        Up,
        Down
    }
}
