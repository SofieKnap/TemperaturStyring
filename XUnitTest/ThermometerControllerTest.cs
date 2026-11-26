using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TemperaturStyring.Controllers;
using TemperaturStyring.Models;

namespace TemperaturStyring.Tests.Controllers;

public class ThermometerControllerTests
{
    private static ThermometerDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ThermometerDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ThermometerDbContext(options);

        context.Thermometers.Add(new Thermometer(1, 20, 10, 30));
        context.SaveChanges();

        return context;
    }

    [Fact]
    public async Task Get_ReturnsAllThermometers()
    {
        using var context = CreateContext();
        var controller = new ThermometerController(context);

        var result = await controller.Get();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var thermometers = Assert.IsAssignableFrom<IEnumerable<Thermometer>>(
            okResult.Value);

        Assert.Single(thermometers);
    }

    [Fact]
    public async Task Get_ReturnsEmptyCollectionWhenNoThermometers()
    {
        using var context = CreateContext();
        var controller = new ThermometerController(context);

        await controller.Delete(1);
        var result = await controller.Get();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var thermometers = Assert.IsAssignableFrom<IEnumerable<Thermometer>>(
            okResult.Value);

        Assert.Empty(thermometers);
    }

    [Fact]
    public async Task Get_WithUnknownId_ReturnsNotFound()
    {
        using var context = CreateContext();
        var controller = new ThermometerController(context);

        var result = await controller.Get(999);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Get_WithKnownId_ReturnsThermometer()
    {
        using var context = CreateContext();
        var controller = new ThermometerController(context);

        var result = await controller.Get(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var thermometer = Assert.IsType<Thermometer>(okResult.Value);

        Assert.Equal(1, thermometer.ThermometerId);
    }

    [Fact]
    public async Task Post_WithValidThermometer_ReturnsCreated()
    {
        using var context = CreateContext();
        var controller = new ThermometerController(context);

        var thermometer = new Thermometer(0, 20, 15, 25);

        var result = await controller.Post(thermometer);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);

        Assert.Equal(201, createdResult.StatusCode);
        Assert.NotEqual(0, thermometer.ThermometerId);
    }

    [Fact]
    public async Task Post_WithInvalidTemperatureLimits_ReturnsBadRequest()
    {
        using var context = CreateContext();
        var controller = new ThermometerController(context);

        var thermometer = new Thermometer(0, 20, 5, 25);

        var result = await controller.Post(thermometer);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(
            result.Result);

        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task Post_WithNonFiniteTemperature_ReturnsBadRequest()
    {
        using var context = CreateContext();
        var controller = new ThermometerController(context);

        var thermometer = new Thermometer(0, double.NaN, 15, 25);

        var result = await controller.Post(thermometer);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task Put_WithUnknownId_ReturnsNotFound()
    {
        using var context = CreateContext();
        var controller = new ThermometerController(context);

        var thermometer = new Thermometer(999, 20, 15, 25);

        var result = await controller.Put(999, thermometer);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Put_WithValidThermometer_ReturnsNoContent()
    {
        using var context = CreateContext();
        var controller = new ThermometerController(context);

        var updatedThermometer = new Thermometer(1, 22, 15, 25);

        var result = await controller.Put(1, updatedThermometer);

        Assert.IsType<NoContentResult>(result);

        var thermometer = await context.Thermometers.FindAsync(1);

        Assert.NotNull(thermometer);
        Assert.Equal(22, thermometer.CurrentTemp);
        Assert.Equal(15, thermometer.MinTemp);
        Assert.Equal(25, thermometer.MaxTemp);
    }

    [Fact]
    public async Task Put_WithInvalidTemperatureLimits_ReturnsBadRequest()
    {
        using var context = CreateContext();
        var controller = new ThermometerController(context);

        var thermometer = new Thermometer(1, 22, 5, 25);

        var result = await controller.Put(1, thermometer);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task UpdateLimits_WithValidLimits_ReturnsUpdatedThermometer()
    {
        using var context = CreateContext();
        var controller = new ThermometerController(context);

        var request = new TemperatureLimitsRequest(12, 28);

        var result = await controller.UpdateLimits(1, request);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var thermometer = Assert.IsType<Thermometer>(okResult.Value);

        Assert.Equal(12, thermometer.MinTemp);
        Assert.Equal(28, thermometer.MaxTemp);
    }

    [Fact]
    public async Task UpdateLimits_WithMissingLimits_ReturnsBadRequest()
    {
        using var context = CreateContext();
        var controller = new ThermometerController(context);

        var request = new TemperatureLimitsRequest(null, 28);

        var result = await controller.UpdateLimits(1, request);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task Delete_WithUnknownId_ReturnsNotFound()
    {
        using var context = CreateContext();
        var controller = new ThermometerController(context);

        var result = await controller.Delete(999);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_WithExistingId_ReturnsNoContent()
    {
        using var context = CreateContext();
        var controller = new ThermometerController(context);

        var result = await controller.Delete(1);

        Assert.IsType<NoContentResult>(result);
        Assert.Null(await context.Thermometers.FindAsync(1));
    }
}