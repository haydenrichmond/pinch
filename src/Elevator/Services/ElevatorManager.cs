using Elevator.Configuration;
using Elevator.Models;
using Microsoft.Extensions.Options;

namespace Elevator.Services
{
    public interface IElevatorManager
    {
        void Start();
        void Stop();

        public Building _building { get; }
    }
    public class ElevatorManager : IElevatorManager
    {
        private readonly IElevatorDispatcherService _elevatorDispatcherService;
        private readonly IOptions<BuildingOptions> _options;

        public ElevatorManager(IOptions<BuildingOptions> config, IElevatorDispatcherService elevatorDispatcherService)
        {
            _options = config;
            _elevatorDispatcherService = elevatorDispatcherService;

        }

        private Building Building;
        public Building _building { get => Building; }

        public void Start()
        {
            Building = new Building(_options);
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
