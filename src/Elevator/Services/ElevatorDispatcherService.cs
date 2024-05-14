
namespace Elevator.Services
{
    public interface IElevatorDispatcherService
    {
        void Dispatch(Elevator.Models.Elevator elevator, int floor);
    }
    public class ElevatorDispatcherService : IElevatorDispatcherService
    {
        public void Dispatch(Models.Elevator elevator, int floor)
        {
            throw new NotImplementedException();
        }
    }
}
