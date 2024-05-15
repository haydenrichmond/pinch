using Elevator.Configuration;
using Microsoft.Extensions.Options;

namespace Elevator.Models
{
    public class Door(IOptions<BuildingOptions> config)
    {
        private readonly BuildingOptions _config = config.Value ?? throw new Exception($"Missing {nameof(BuildingOptions)}");

        public DoorStatus Status { get; set; }

        public async Task Open()
        {
            Console.WriteLine("Doors opening...");
            Status = DoorStatus.Opening;
            // Simulate time taken to move one floor
            await Task.Delay(_config.DoorOpenCloseTimeMilliseconds);
            Status = DoorStatus.Open;
        }
        public async Task Close()
        {
            Console.WriteLine("Doors closing...");
            Status = DoorStatus.Closing;
            await Task.Delay(_config.DoorOpenCloseTimeMilliseconds);
            Status = DoorStatus.Closed;
        }
    }


    public enum DoorStatus
    {
        Open,
        Closed,
        Closing,
        Opening
    }
}
