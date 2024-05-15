
namespace Elevator.Events
{
    public class MoveToTargetFloorEventArgs : EventArgs
    {
        public int TargetFloor { get; set; }
        public Elevator.Models.Elevator Elevator { get; set; }

        public MoveToTargetFloorEventArgs(Elevator.Models.Elevator elevator, int targetFloor)
        {
            Elevator = elevator;
            TargetFloor = targetFloor;
        }
    }
}
