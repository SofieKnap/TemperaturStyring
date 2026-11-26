using Microsoft.AspNetCore.Mvc;
using TemperaturStyring.Controllers;
using TemperaturStyring.Models;

namespace TemperaturStyring.Tests.Controllers;

public class ThermometerControllerTests
{
    [Fact]
    public void Get_ReturnsOkWithThermometers()
    {
        // Arrange
        var controller = new ThermometerController();

        // Act
        var result = controller.Get();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var thermometers = Assert.IsAssignableFrom<IEnumerable<Thermometer>>(
            okResult.Value);

        Assert.Single(thermometers);
    }

    [Fact]
    public void Get_returnsNoContentWhenNoThermometers()
    {
        // Arrange
        var controller = new ThermometerController();

        // Remove the default thermometer
        controller.Delete(1);

        // Act
        var result = controller.Get();

        // Assert
        var noContentResult = Assert.IsType<NoContentResult>(result.Result);

        Assert.Equal(204, noContentResult.StatusCode);
    }

    [Fact]
    public void Get_WithUnknownId_ReturnsNotFound()
    {
        // Arrange
        var controller = new ThermometerController();

        // Act
        var result = controller.Get(999);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public void Get_WithKnownId_ReturnsOkWithThermometer()
    {
        // Arrange
        var controller = new ThermometerController();
        // Act
        var result = controller.Get(1);
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var thermometer = Assert.IsType<Thermometer>(okResult.Value);
        Assert.Equal(1, thermometer.ThermometerId);
    }

    [Fact]
    public void Post_WithExistingId_ReturnsConflict()
    {
        // Arrange
        var controller = new ThermometerController();
        var thermometer = new Thermometer(
            thermometerId: 1,
            currentTemperature: 20,
            minTemperature: 15,
            maxTemperature: 25);
        // Act
        var result = controller.Post(thermometer);
        // Assert
        var conflictResult = Assert.IsType<ConflictObjectResult>(result.Result);
        Assert.Equal(409, conflictResult.StatusCode);
    }

    [Fact]
    public void Post_WithValidThermometer_ReturnsCreated()
    {
        // Arrange
        var controller = new ThermometerController();
        var thermometer = new Thermometer(
            thermometerId: 2,
            currentTemperature: 20,
            minTemperature: 15,
            maxTemperature: 25);

        // Act
        var result = controller.Post(thermometer);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);

        Assert.Equal(201, createdResult.StatusCode);
        Assert.Equal(thermometer, createdResult.Value);
    }

    [Fact]
    public void Post_WithInvalidTemperatureLimits_ReturnsBadRequest()
    {
        // Arrange
        var controller = new ThermometerController();
        var thermometer = new Thermometer(
            thermometerId: 2,
            currentTemperature: 20,
            minTemperature: 5,
            maxTemperature: 25);

        // Act
        var result = controller.Post(thermometer);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(
            result.Result);

        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public void Put_WithUnknownId_ReturnsNotFound()
    {
        // Arrange
        var controller = new ThermometerController();
        var thermometer = new Thermometer(999, 20, 15, 25);

        // Act
        var result = controller.Put(999, thermometer);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Put_WithValidThermometer_ReturnsNoContent()
    {
        // Arrange
        var controller = new ThermometerController();
        var thermometer = new Thermometer(1, 22, 15, 25);
        // Act
        var result = controller.Put(1, thermometer);
        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void Put_WithInvalidTemperatureLimits_ReturnsBadRequest()
    {
        // Arrange
        var controller = new ThermometerController();
        var thermometer = new Thermometer(1, 22, 5, 25);
        // Act
        var result = controller.Put(1, thermometer);
        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public void Delete_WithUnknownId_ReturnsNotFound()
    {
        // Arrange
        var controller = new ThermometerController();
        // Act
        var result = controller.Delete(999);
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    [Fact]
    public void Delete_WithExistingId_ReturnsNoContent()
    {
        // Arrange
        var controller = new ThermometerController();

        // Act
        var result = controller.Delete(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}