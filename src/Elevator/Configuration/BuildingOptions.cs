namespace Elevator.Configuration
{
    public class BuildingOptions
    {
        public const string SectionName = "Building";

        /// <summary>
        /// Minimum 1 required. 0 denotes one floor, Ground.
        /// </summary>
        public required int TotalFloors { get; set; }



        /// <summary>
        /// Minimum 1 required
        /// </summary>
        public required int TotalElevators { get; set; }

        public required int DoorOpenCloseTimeMilliseconds { get; set; }
        public required int TimeToTravelBetweenFloorsMilliseconds { get; set; }

        public required int PassengerLoadUnloadTimeMilliseconds { get; set; }

        public required int FloorStopBuffer { get; set; }
    }
}
