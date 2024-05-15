using Elevator.Configuration;
using Microsoft.Extensions.Options;

namespace Elevator.Models
{
    public class Elevator
    {
        private readonly BuildingOptions _config;
        public Elevator(IOptions<BuildingOptions> config, Button[] buttons)
        {
            _config = config.Value;
            Buttons = buttons;
        }

        private object _lockObj = new object();
        private List<int> _upDirectionFloors = new();
        private List<int> _downDirectionFloors = new();

        public Button[] Buttons { get; set; }
        public int IdNumber { get; set; }
        public Door Door { get; set; }
        /// <summary>
        /// 0 denotes floor G
        /// </summary>
        public int CurrentFloor { get; set; }

        /// <summary>
        /// Null denotes no motion.
        /// </summary>
        public DirectionOfTravel? DirectionOfTravel { get; set; }
        public bool Moving = false; // Can derive from DirectionOfTravel

        public event Action<int> FloorChanged;

        public async Task QueueFloor(int targetFloor, bool? upButtonPressed)
        {
            lock (_lockObj)
            {
                if (DirectionOfTravel == null)
                {
                    if (upButtonPressed == null)
                    {
                        if (targetFloor > CurrentFloor)
                        {
                            _upDirectionFloors.Add(targetFloor);
                        }
                        else
                        {
                            _downDirectionFloors.Add(targetFloor);
                        }
                    }
                    else
                    {

                        if (upButtonPressed == true)
                        {
                            _upDirectionFloors.Add(targetFloor);
                        }
                        else
                        {
                            _downDirectionFloors.Add(targetFloor);
                        }
                    }
                }
                else
                {
                    // Same code block repeated
                    if (upButtonPressed == null)
                    {
                        // use the direction of travel to determine where to send it??
                        if (DirectionOfTravel == Models.DirectionOfTravel.Up)
                        {
                            _upDirectionFloors.Add(targetFloor);
                        }
                        else
                        {
                            _downDirectionFloors.Add(targetFloor);
                        }
                    }
                    else
                    {
                        // always do whatever the requested direction was
                        if (upButtonPressed == true)
                        {
                            _upDirectionFloors.Add(targetFloor);
                        }
                        else
                        {
                            _downDirectionFloors.Add(targetFloor);
                        }
                    }
                }

                _upDirectionFloors.Sort();
                _downDirectionFloors.Sort();
                _downDirectionFloors.Reverse();
            }
            await ProcessRequests();
        }

        public async Task ProcessRequests()
        {
            Console.WriteLine("Attempted to process requests...");
            if (Moving)
            {
                Console.WriteLine("Elevator already moving so ignore...");
                return;
            }

            Console.WriteLine("Elevator has been woken up...");
            Moving = true;

            while (true)
            {
                int nextFloor;
                lock (_lockObj)
                {
                    if (_upDirectionFloors.Count == 0 && _downDirectionFloors.Count == 0)
                    {
                        DirectionOfTravel = null;
                        Moving = false;
                        return;
                    }

                    if ((DirectionOfTravel == Models.DirectionOfTravel.Up || DirectionOfTravel == null) && _upDirectionFloors.Count != 0)
                    {
                        nextFloor = _upDirectionFloors.First();
                        _upDirectionFloors.Remove(nextFloor);
                    }
                    else
                    {
                        nextFloor = _downDirectionFloors.First();
                        _downDirectionFloors.Remove(nextFloor);
                    }
                }

                await MoveToFloor(nextFloor);
            }
        }

        private async Task MoveToFloor(int floor)
        {
            while (CurrentFloor != floor)
            {
                if (CurrentFloor < floor)
                {
                    DirectionOfTravel = Models.DirectionOfTravel.Up;
                    CurrentFloor++;

                }

                else if (CurrentFloor > floor)
                {
                    DirectionOfTravel = Models.DirectionOfTravel.Down;
                    CurrentFloor--;
                }


                FloorChanged.Invoke(CurrentFloor);

                // Simulate time taken to move one floor
                await Task.Delay(_config.TimeToTravelBetweenFloorsMilliseconds);
            }

            Console.WriteLine($"Arrived at floor {floor}. Trigger opening doors");
            await Door.Open();
            Console.WriteLine("Passengers boarding");
            await Task.Delay(_config.PassengerLoadUnloadTimeMilliseconds);
            await Door.Close();
            Console.WriteLine("GO!");

        }
    }

    public enum DirectionOfTravel
    {
        Up,
        Down
    }
}
