using Elevator.Events;

namespace Elevator.Models
{
    public class Floor
    {
        public Floor(int floorNumber)
        {
            FloorNumber = floorNumber;
        }

        public bool UpButtonPressed { get; set; }
        public bool DownButtonPressed { get; set; }

        public int FloorNumber { get; set; }

        public event CallButtonPressedEventHandler CallButtonPressed;

        public void PressUpButton()
        {
            UpButtonPressed = true;
            CallButtonPressed.Invoke(this, new CallButtonPressedEventArgs(this));
        }

        public void PressDownButton()
        {
            DownButtonPressed = true;
            CallButtonPressed.Invoke(this, new CallButtonPressedEventArgs(this));
        }

        public void PassengersPickedUp()
        {
            UpButtonPressed = false;
            DownButtonPressed = false;
        }
    }
}
