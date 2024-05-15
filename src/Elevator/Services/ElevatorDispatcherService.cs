
using Elevator.Events;
using Elevator.Models;

namespace Elevator.Services
{
    public interface IElevatorDispatcherService
    {
        void Dispatch(int floor);
    }
    public class ElevatorDispatcherService : IElevatorDispatcherService
    {

        public void Dispatch(int floorNumber)
        {
            //var availableElevator = Elevators.FirstOrDefault(x => !x._moving
            //||
            //(x.DirectionOfTravel == DirectionOfTravel.Up && x._currentFloor < floorNumber - FloorStopBuffer)
            //||
            //(x.DirectionOfTravel == DirectionOfTravel.Down && x._currentFloor > floorNumber + FloorStopBuffer)
            //);

            //await availableElevator.QueueFloor(floorNumber);
        }
    }
}
