using Elevator.Models;

namespace Elevator.Events
{
    public delegate void ElevatorButtonPressedEventHandler(object sender, ElevatorButtonPressedEventArgs e);

    public class ElevatorButtonPressedEventArgs : EventArgs
    {
        public Button Button { get; set; }

        public ElevatorButtonPressedEventArgs(Button button)
        {
            Button = button;
        }
    }
}
