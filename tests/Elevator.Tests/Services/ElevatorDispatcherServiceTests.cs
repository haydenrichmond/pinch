using Elevator.Configuration;
using Elevator.Models;
using Elevator.Services;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace Elevator.Tests.Services
{
    [TestFixture]
    public class ElevatorDispatcherServiceTests
    {
        private ElevatorDispatcherService _sut;
        private Mock<IOptions<BuildingOptions>> _mockOptions;


        [TestCase]
        public async Task OneElevatorMoving()
        {
            // Arrange
            var elevator = BuildElevator();
            elevator.Moving = true;

            var elevators = new Models.Elevator[] { elevator };

            // Act
            var result = await _sut.Dispatch(elevators, 1, true);

            // Assert
            Assert.That(result.IdNumber == elevator.IdNumber);
        }

        [TestCase]
        public async Task TwoElevatorMoving()
        {
            // Arrange
            var elevator1 = BuildElevator();
            elevator1.Moving = true;
            elevator1.IdNumber = 1;

            var elevator2 = BuildElevator();
            elevator2.Moving = true;
            elevator2.IdNumber = 2;

            var elevators = new Models.Elevator[] { elevator1, elevator2 };

            // Act
            var result = await _sut.Dispatch(elevators, 1, true);

            // Assert
            Assert.That(result.IdNumber == elevator1.IdNumber);
        }

        [TestCase]
        public async Task SecondElevatorIdle()
        {
            // Arrange
            var elevator1 = BuildElevator();
            elevator1.Moving = true;
            elevator1.IdNumber = 1;

            var elevator2 = BuildElevator();
            elevator2.Moving = false;
            elevator2.IdNumber = 2;

            var elevators = new Models.Elevator[] { elevator1, elevator2 };

            // Act
            var result = await _sut.Dispatch(elevators, 1, true);

            // Assert
            Assert.That(result.IdNumber == elevator2.IdNumber);
        }

        private Models.Elevator BuildElevator()
        {
            var elevator = new Mock<Models.Elevator>(_mockOptions.Object, null);
            elevator.Object.FloorChanged += (floor) => Console.WriteLine($"Elevator is now at floor {floor}");
            elevator.Object.Door = new Mock<Door>(_mockOptions.Object).Object;
            return elevator.Object;
        }

        [SetUp]
        protected void SetUp()
        {
            var options = new BuildingOptions()
            {
                DoorOpenCloseTimeMilliseconds = 1,
                PassengerLoadUnloadTimeMilliseconds = 1,
                TimeToTravelBetweenFloorsMilliseconds = 1,
                FloorStopBuffer = 2,
                TotalElevators = 1,
                TotalFloors = 1
            };
            _mockOptions = new Mock<IOptions<BuildingOptions>>();
            _mockOptions.Setup(o => o.Value).Returns(options);
            _sut = new ElevatorDispatcherService(_mockOptions.Object);
        }

    }
}
