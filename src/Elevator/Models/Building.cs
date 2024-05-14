using Elevator.Configuration;
using Microsoft.Extensions.Options;

namespace Elevator.Models
{
    public class Building
    {
        public Building(IOptions<BuildingOptions> config)
        {
            Floors = new Floor[config.Value.TotalFloors];
            for (var i = 0; i < config.Value.TotalFloors; i++)
            {
                Floors[i] = new Floor(i);
            }

            Elevators = new Elevator[config.Value.TotalElevators];
            for (var i = 0; i < config.Value.TotalElevators; i++)
            {
                Elevators[i] = new Elevator(config);
            }
        }

        public Floor[] Floors { get; set; }

        public Elevator[] Elevators { get; set; }
    }
}
