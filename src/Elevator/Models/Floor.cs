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

        public readonly int FloorNumber;

        public event Action<int, Floor> CallButtonPressed;

        public async Task PressUpButton()
        {
            if (FloorNumber >= 10) //TODO get from config
            {
                throw new ArgumentOutOfRangeException("Top floor is unable to go up.");
            }


            UpButtonPressed = true;
            CallButtonPressed.Invoke(FloorNumber, this);
        }

        public async Task PressDownButton()
        {
            if (FloorNumber <= 0)
            {
                throw new ArgumentOutOfRangeException("Ground floor is unable to go down.");
            }

            DownButtonPressed = true;
            CallButtonPressed.Invoke(FloorNumber, this);
        }

        public async Task PassengersPickedUp()
        {
            UpButtonPressed = false;
            DownButtonPressed = false;
        }
    }
}
