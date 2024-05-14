namespace Elevator.Models
{
    public class Button
    {
        public bool UpButtonPressed {  get; set; }
        public bool DownButtonPressed { get; set; }

        public void PressUpButton()
        {
            UpButtonPressed = true;
        }

        public void PressDownButton()
        {
            DownButtonPressed = true;
        }

        public void PassengersPickedUp()
        {
            UpButtonPressed = false;
            DownButtonPressed = false;
        }
    }
}
