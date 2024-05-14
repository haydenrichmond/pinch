using Elevator.Configuration;
using Microsoft.Extensions.Options;

namespace Elevator.Models
{
    public class Elevator(IOptions<BuildingOptions> config)
    {
        private readonly BuildingOptions _config = config.Value ?? throw new Exception($"Missing {nameof(BuildingOptions)}");

        public ElevatorStatus Status { get; set; }

        public Door Door { get; set; }

        public int MaxFloor => _config.TotalFloors;

        /// <summary>
        /// 0 denotes floor G
        /// </summary>
        public int CurrentFloor { get; set; }

        public int? TargetFloor { get; set; }


        public DirectionOfTravel? DirectionOfTravel { get; set; }


        public decimal CurrentSpeed { get; set; }

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
