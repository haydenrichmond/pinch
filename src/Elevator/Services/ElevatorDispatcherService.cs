
using Elevator.Events;

namespace Elevator.Services
{
    public interface IElevatorDispatcherService
    {
        void Dispatch(Elevator.Models.Elevator elevator, int floor);
    }
    public class ElevatorDispatcherService : IElevatorDispatcherService
    {

        private static void Button_CallButtonPressed(object sender, CallButtonPressedEventArgs e)
        {
            Console.WriteLine($"CallButtonPressed");
        }

        public void Dispatch(Models.Elevator elevator, int floor)
        {
            throw new NotImplementedException();
        }
    }
}
