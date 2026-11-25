using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemperaturStyring.Models;

namespace TemperaturStyring.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ThermometerController : ControllerBase
{
    private readonly List<Thermometer> thermometers =
    [
        new Thermometer(1, 20.0, 10.0, 30.0)
    ];

    // GET: api/Thermometer
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<IEnumerable<Thermometer>> Get()
    {
        if (thermometers.Count == 0)
        {
            return NoContent();
        }

        return Ok(thermometers);
    }

    // GET: api/Thermometer/1
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Thermometer> Get(int id)
    {
        Thermometer? thermometer = thermometers
            .FirstOrDefault(t => t.ThermometerId == id);

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
    public ActionResult<Thermometer> Post(Thermometer thermometer)
    {
        if (thermometers.Any(t => t.ThermometerId == thermometer.ThermometerId))
        {
            return Conflict($"A thermometer with ID {thermometer.ThermometerId} already exists.");
        }

        try
        {
            thermometer.ValidateTemp();
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }

        thermometers.Add(thermometer);

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
    public IActionResult Put(int id, Thermometer updatedThermometer)
    {
        Thermometer? thermometer = thermometers
            .FirstOrDefault(t => t.ThermometerId == id);

        if (thermometer is null)
        {
            return NotFound();
        }

        try
        {
            thermometer.CurrentTemp = updatedThermometer.CurrentTemp;
            thermometer.UpdateTempLimits(
                updatedThermometer.MinTemp,
                updatedThermometer.MaxTemp);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }

        return NoContent();
    }

    // DELETE: api/Thermometer/1
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        Thermometer? thermometer = thermometers
            .FirstOrDefault(t => t.ThermometerId == id);

        if (thermometer is null)
        {
            return NotFound();
        }

        thermometers.Remove(thermometer);

        return NoContent();
    }
}