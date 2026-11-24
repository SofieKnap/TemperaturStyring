using TemperaturStyring.Models;

namespace XUnitTest
{
    public class ThermometerTests
    {
        [Fact]
        public void InvalidMinTemp()
        {
            // Arrange
            Thermometer thermometer = new Thermometer(1, 22, 1, 25);

            // Act
            Action act = () => thermometer.ValidateTemp();

            // Assert
            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void InvalidMaxTemp()
        {
            // Arrange
            Thermometer thermometer = new Thermometer(1, 22, 15, 60);

            // Act
            Action act = () => thermometer.ValidateTemp();

            // Assert
            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void MinTempGreaterThanMaxTemp()
        {
            // Arrange
            Thermometer thermometer = new Thermometer(1, 22, 25, 15);

            // Act
            Action act = () => thermometer.ValidateTemp();

            // Assert
            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void MinTempEqualToMaxTemp()
        {
            // Arrange
            Thermometer thermometer = new Thermometer(1, 22, 20, 20);

            // Act
            Action act = () => thermometer.ValidateTemp();
            
            // Assert
            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ValidTemp()
        {
            // Arrange
            Thermometer thermometer = new Thermometer(1, 22, 15, 25);

            // Act
            Action act = () => thermometer.ValidateTemp();
            var exception = Record.Exception(act);

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void ValidBoundaryTemp()
        {
            // Arrange
            Thermometer thermometer = new Thermometer(1, 22, 10, 30);
           
            // Act
            Action act = () => thermometer.ValidateTemp();
            var exception = Record.Exception(act);
            
            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void UpdateValidTempLimits()
        {
            // Arrange
            Thermometer thermometer = new Thermometer(1, 22, 15, 25);

            // Act
            thermometer.UpdateTempLimits(18, 28);

            // Assert
            Assert.Equal(18, thermometer.MinTemp);
            Assert.Equal(28, thermometer.MaxTemp);
        }

        [Fact]
        public void UpdateInvalidTempLimits()
        {
            // Arrange
            Thermometer thermometer = new Thermometer(1, 22, 15, 25);

            // Act
            Action act = () => thermometer.UpdateTempLimits(5, 35);

            // Assert
            Assert.Throws<ArgumentException>(act);
            Assert.Equal(15, thermometer.MinTemp);
            Assert.Equal(25, thermometer.MaxTemp);
        }


    }
}
