using Elevator.Configuration;
using Elevator.Models;
using Microsoft.Extensions.Options;

namespace Elevator.Services
{
    public interface IElevatorDispatcherService
    {
        Task<Models.Elevator> Dispatch(Models.Elevator[] elevators, int floorNumber, bool upButtonPressed);
    }
    public class ElevatorDispatcherService : IElevatorDispatcherService
    {
        private readonly BuildingOptions _options;
        public ElevatorDispatcherService(IOptions<BuildingOptions> options)
        {
            _options = options.Value;
        }

        public async Task<Models.Elevator> Dispatch(Models.Elevator[] elevators, int floorNumber, bool upButtonPressed)
        {
            var availableElevator = elevators.
                OrderBy(x => x.Moving) // Idle elevators first
                .ThenBy(x =>
                (x.DirectionOfTravel == DirectionOfTravel.Up && x.CurrentFloor < floorNumber - _options.FloorStopBuffer)
                ||
                (x.DirectionOfTravel == DirectionOfTravel.Down && x.CurrentFloor > floorNumber + _options.FloorStopBuffer)
                )
                .ThenBy(x => x.IdNumber)
            .First();

            await availableElevator.QueueFloor(floorNumber, upButtonPressed);

            return availableElevator;
        }
    }
}
