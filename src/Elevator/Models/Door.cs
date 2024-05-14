using Elevator.Configuration;
using Microsoft.Extensions.Options;

namespace Elevator.Models
{
    public class Door(IOptions<BuildingOptions> config)
    {
        private readonly BuildingOptions _config = config.Value ?? throw new Exception($"Missing {nameof(BuildingOptions)}");

        public DoorStatus Status { get; set; }

        public void Open()
        {
            Status = DoorStatus.Opening;
            Thread.Sleep(_config.DoorOpenCloseTimeMilliseconds);
            Status = DoorStatus.Open;
        }
        public void Close()
        {
            Status = DoorStatus.Closing;
            Thread.Sleep(_config.DoorOpenCloseTimeMilliseconds);
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
