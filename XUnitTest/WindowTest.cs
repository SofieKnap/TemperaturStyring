using System;
using System.Collections.Generic;
using System.Text;
using TemperaturStyring.Models;

namespace XUnitTest
{
    public class WindowTest
    {
        [Fact]
        public void OpenWindow()
        {
            // Arrange
            Window window = new Window();

            // Act
            window.OpenWindow();

            // Assert
            Assert.True(window.WindowStatus);
        }

        [Fact]
        public void CloseWindow()
        {
            // Arrange
            Window window = new Window();
            window.OpenWindow();

            // Act
            window.CloseWindow();

            // Assert
            Assert.False(window.WindowStatus);
        }
    }
}
