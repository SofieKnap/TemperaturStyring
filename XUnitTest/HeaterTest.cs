using TemperaturStyring.Models;
using Xunit;

namespace XUnitTest
{
    public class HeaterTest
    {
        [Fact]
        public void TurnOnHeater()
        {
            // Arrange
            Heater heater = new Heater();

            // Act
            heater.TurnOnHeater();

            // Assert
            Assert.True(heater.HeaterStatus);
        }

        [Fact]
        public void TurnOffHeater()
        {
            // Arrange
            Heater heater = new Heater();
            heater.TurnOnHeater();

            // Act
            heater.TurnOffHeater();

            // Assert
            Assert.False(heater.HeaterStatus);
        }
    }
} 