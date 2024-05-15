using Elevator.Configuration;
using Elevator.Events;
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


        public bool _moving = false;
        //private List<int> _targetFloors = new();


        private List<int> _upDirectionFloors = new();
        private List<int> _downDirectionFloors = new();

        public Button[] Buttons { get; set; }

        public int IdNumber { get; set; }


        //TODO
        public Door Door { get; set; }



        public int MaxFloor => _config.TotalFloors;
        public int MinFloor => 0;

        /// <summary>
        /// 0 denotes floor G
        /// </summary>
        public int _currentFloor { get; set; }



        private object lockObj = new object();


        public DirectionOfTravel? DirectionOfTravel { get; set; }



        //public event EventHandler<MoveToTargetFloorEventArgs> MoveToTargetFloor;
        public event Action<int> FloorChanged;

        public async Task QueueFloor(int targetFloor, bool? upButtonPressed)
        {
            lock (lockObj)
            {
                //if (_upDirectionFloors.Contains(targetFloor))
                //{
                //    // No need to queue
                //    return;
                //}

                if (DirectionOfTravel == null)
                {
                    if (upButtonPressed == null)
                    {
                        if (targetFloor > _currentFloor)
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

                    // Attempt 2
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



                    //Attempt 1
                    //// Scan search, go to most extreme floor then reverse direction

                    //var maxTargetFloor = _targetFloors.Max();
                    //var maxNumberIndex = _targetFloors.FindIndex(x => x == maxTargetFloor);

                    ////TODO Optimise for better performance
                    //// Split the list into two parts
                    //List<int> upSequence = _targetFloors.Skip(maxNumberIndex).ToList();
                    //List<int> downSequence = _targetFloors.Take(maxNumberIndex + 1).Reverse().ToList();

                    //if (upButtonPressed == null)
                    //{
                    //    if (targetFloor > _currentFloor)
                    //    {
                    //        upSequence.Add(targetFloor);
                    //    }
                    //    else if (targetFloor < _currentFloor)
                    //    {
                    //        downSequence.Add(targetFloor);
                    //    }
                    //}
                    //else
                    //{
                    //    if (upButtonPressed == true)
                    //    {
                    //        upSequence.Add(targetFloor);
                    //    }
                    //    else
                    //    {
                    //        downSequence.Add(targetFloor);
                    //    }
                    //}

                    //upSequence.Sort();
                    //downSequence.Sort();
                    //downSequence.Reverse();
                    //if (DirectionOfTravel == Models.DirectionOfTravel.Up)
                    //{
                    //    upSequence.AddRange(downSequence);
                    //    _targetFloors = upSequence.Distinct().ToList();
                    //}
                    //else if (DirectionOfTravel == Models.DirectionOfTravel.Down)
                    //{
                    //    downSequence.AddRange(upSequence);
                    //    _targetFloors = downSequence.Distinct().ToList();
                    //}
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
            if (_moving)
            {
                Console.WriteLine("Elevator already moving so ignore...");
                return;
            }

            Console.WriteLine("Elevator has been woken up...");
            _moving = true;

            while (true)
            {
                int nextFloor;
                lock (lockObj)
                {
                    if (_upDirectionFloors.Count == 0 && _downDirectionFloors.Count == 0)
                    {
                        DirectionOfTravel = null;
                        _moving = false;
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
            while (_currentFloor != floor)
            {
                if (_currentFloor < floor)
                {
                    DirectionOfTravel = Models.DirectionOfTravel.Up;
                    _currentFloor++;

                }

                else if (_currentFloor > floor)
                {
                    DirectionOfTravel = Models.DirectionOfTravel.Down;
                    _currentFloor--;
                }


                FloorChanged.Invoke(_currentFloor);

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

        //public async Task MoveToTargetFloorMethod(int targetFloor)
        //{
        //    MoveToTargetFloor.Invoke(this, new MoveToTargetFloorEventArgs(this, targetFloor));
        //}

        //public void MoveToTargetFloor(int targetFloor)
        //{
        //    if (targetFloor > MaxFloor || targetFloor < MinFloor)
        //    {
        //        throw new ArgumentOutOfRangeException(nameof(targetFloor), targetFloor.ToString());
        //    }


        //    if (CurrentFloor == targetFloor)
        //    {
        //        return;
        //    }

        //    TargetFloor = targetFloor;
        //    Status = ElevatorStatus.Moving;
        //    var moveUp = CurrentFloor < targetFloor;

        //    if (moveUp)
        //    {
        //        Console.WriteLine($"Elevator moving up to floor {targetFloor}.");
        //        DirectionOfTravel = Models.DirectionOfTravel.Up;

        //        while (CurrentFloor < targetFloor)
        //        {
        //            Console.WriteLine($"Elevator on floor {CurrentFloor}.");
        //            Thread.Sleep(_config.TimeToTravelBetweenFloorsMilliseconds);
        //            CurrentFloor++;
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine($"Elevator moving down to floor {targetFloor}.");
        //        DirectionOfTravel = Models.DirectionOfTravel.Down;
        //        while (CurrentFloor > targetFloor)
        //        {
        //            Console.WriteLine($"Elevator on floor {CurrentFloor}.");
        //            Thread.Sleep(_config.TimeToTravelBetweenFloorsMilliseconds);
        //            CurrentFloor--;
        //        }
        //    }

        //    Buttons[targetFloor].ButtonActive = false;
        //    CurrentFloor = targetFloor;
        //    TargetFloor = null;
        //    Status = ElevatorStatus.Idle;
        //    DirectionOfTravel = null;
        //    Console.WriteLine($"Elevator on floor {CurrentFloor}.");
        //}

    }

    public enum DirectionOfTravel
    {
        Up,
        Down
    }
}
