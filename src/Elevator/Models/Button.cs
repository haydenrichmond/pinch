using Elevator.Events;
using System.Runtime.CompilerServices;

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

        public bool ButtonActive { get; set; }

        public event EventHandler<ElevatorButtonPressedEventArgs> ElevatorButtonPressed;

        public async Task PressButton()
        {
            if (!ButtonActive)
            {
                ButtonActive = true;
                ElevatorButtonPressed.Invoke(this, new ElevatorButtonPressedEventArgs(this));
            }
            else
            {
                Console.WriteLine("Do nothing. Button has already been pushed and not yet reached floor.");
            }
        }
    }
}
