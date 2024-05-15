using Elevator.Configuration;
using Elevator.Services;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace Elevator.Tests.Services
{
    [TestFixture]
    public class ElevatorManagerTests
    {
        private ElevatorManager _sut;
        private BuildingOptions _options;
        private Mock<IOptions<BuildingOptions>> _mockOptions;

        [TestCase(10)]
        [TestCase(6)]
        [TestCase(1)]
        public async Task CorrectNumberFloors(int totalFloors)
        {
            // Arrange
            _options.TotalFloors = totalFloors;
            _mockOptions.Setup(o => o.Value).Returns(_options);

            // Act
            _sut.Start();

            // Assert
            Assert.That(_sut.Floors.Count() == totalFloors + 1);
        }

        [TestCase(10)]
        [TestCase(6)]
        [TestCase(1)]
        public async Task CorrectNumberElevators(int totalElevators)
        {
            // Arrange
            _options.TotalElevators = totalElevators;
            _mockOptions.Setup(o => o.Value).Returns(_options);

            // Act
            _sut.Start();

            // Assert
            Assert.That(_sut.Elevators.Count() == totalElevators);
        }


        [SetUp]
        protected void SetUp()
        {
            _options = new BuildingOptions()
            {
                DoorOpenCloseTimeMilliseconds = 1,
                PassengerLoadUnloadTimeMilliseconds = 1,
                TimeToTravelBetweenFloorsMilliseconds = 1,
                FloorStopBuffer = 2,
                TotalElevators = 1,
                TotalFloors = 1
            };
            _mockOptions = new Mock<IOptions<BuildingOptions>>();
            _mockOptions.Setup(o => o.Value).Returns(_options);
            var dispatcher = new Mock<IElevatorDispatcherService>();
            _sut = new ElevatorManager(_mockOptions.Object, dispatcher.Object);
        }
    }
}
