using Elevator.Models;

namespace Elevator.Events
{
    //public delegate void CallButtonPressedEventHandler(object sender, CallButtonPressedEventArgs e);

    public class CallButtonPressedEventArgs : EventArgs
    {
        public Floor Floor { get; set; }

        public CallButtonPressedEventArgs(Floor floor)
        {
            Floor = floor;
        }
    }
}
