using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TemperaturStyring.Models;

namespace TemperaturStyring.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ThermometerController : ControllerBase
{
    private readonly ThermometerDbContext context;

    public ThermometerController(ThermometerDbContext context)
    {
        this.context = context;
    }


    // GET: api/Thermometer
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<Thermometer>>> Get()
    {
        var thermometers = await context.Thermometers
            .AsNoTracking()
            .OrderBy(thermometer => thermometer.ThermometerId)
            .ToListAsync();

        return Ok(thermometers);
    }

    // GET: api/Thermometer/1
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Thermometer>> Get(int id)
    {
        Thermometer? thermometer = await context.Thermometers.FindAsync(id);

        if (thermometer is null)
        {
            return NotFound();
        }

        return Ok(thermometer);
    }

    // POST: api/Thermometer
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Thermometer>> Post(Thermometer thermometer)
    {
        if (!double.IsFinite(thermometer.CurrentTemp))
        {
            return BadRequest("Den aktuelle temperatur skal være et gyldigt tal.");
        }

        try
        {
            thermometer.ValidateTemp();
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }

        thermometer.ThermometerId = 0;
        context.Thermometers.Add(thermometer);
        await context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(Get),
            new { id = thermometer.ThermometerId },
            thermometer);
    }



    // PUT: api/Thermometer/1
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Put(int id, Thermometer updatedThermometer)
    {
        Thermometer? thermometer = await context.Thermometers.FindAsync(id);

        if (thermometer is null)
        {
            return NotFound();
        }

        if (!double.IsFinite(updatedThermometer.CurrentTemp))
        {
            return BadRequest("Den aktuelle temperatur skal være et gyldigt tal.");
        }

        try
        {
            thermometer.UpdateTempLimits(
                updatedThermometer.MinTemp,
                updatedThermometer.MaxTemp);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }

        thermometer.CurrentTemp = updatedThermometer.CurrentTemp;
        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPut("{id:int}/limits")]
    public async Task<ActionResult<Thermometer>> UpdateLimits(
        int id,
        TemperatureLimitsRequest request)
    {
        if (request.MinTemp is not double minTemp ||
            request.MaxTemp is not double maxTemp)
        {
            return BadRequest("Udfyld både den nedre og den øvre temperaturgrænse.");
        }

        var thermometer = await context.Thermometers.FindAsync(id);

        if (thermometer is null)
        {
            return NotFound();
        }

        try
        {
            thermometer.UpdateTempLimits(minTemp, maxTemp);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }

        await context.SaveChangesAsync();

        return Ok(thermometer);
    }



    // DELETE: api/Thermometer/1
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var thermometer = await context.Thermometers.FindAsync(id);

        if (thermometer is null)
        {
            return NotFound();
        }

        context.Thermometers.Remove(thermometer);
        await context.SaveChangesAsync();

        return NoContent();
    }


}

public record TemperatureLimitsRequest(double? MinTemp, double? MaxTemp);