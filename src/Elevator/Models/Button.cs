using Elevator.Events;

namespace Elevator.Models
{
    public class Button
    {
        public Button(int floorNumber)
        {
            FloorNumber = floorNumber;
        }

        public int FloorNumber { get; set; }
        public Elevator Elevator { get; set; }

        public event ElevatorButtonPressedEventHandler ElevatorButtonPressed;

        public void PressButton()
        {
            ElevatorButtonPressed.Invoke(this, new ElevatorButtonPressedEventArgs(this));
        }
    }
}
