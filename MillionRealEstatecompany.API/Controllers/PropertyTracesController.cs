using Microsoft.AspNetCore.Mvc;
using MillionRealEstatecompany.API.DTOs;
using MillionRealEstatecompany.API.Interfaces;

namespace MillionRealEstatecompany.API.Controllers;

/// <summary>
/// Controller for managing property trace operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PropertyTracesController : ControllerBase
{
    private readonly IPropertyTraceService _propertyTraceService;
    private readonly ILogger<PropertyTracesController> _logger;

    public PropertyTracesController(IPropertyTraceService propertyTraceService, ILogger<PropertyTracesController> logger)
    {
        _propertyTraceService = propertyTraceService;
        _logger = logger;
    }

    /// <summary>
    /// Get traces by property ID
    /// </summary>
    [HttpGet("by-property/{propertyId}")]
    public async Task<ActionResult<IEnumerable<PropertyTraceDto>>> GetTracesByProperty(int propertyId)
    {
        try
        {
            var traces = await _propertyTraceService.GetTracesByPropertyAsync(propertyId);
            return Ok(traces);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting traces for property {PropertyId}", propertyId);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get property trace by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<PropertyTraceDto>> GetTrace(int id)
    {
        try
        {
            var trace = await _propertyTraceService.GetTraceByIdAsync(id);
            if (trace == null)
                return NotFound();

            return Ok(trace);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting trace with id {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Crear un nuevo rastro de transacción de propiedad
    /// </summary>
    /// <param name="createTraceDto">Datos del rastro a crear</param>
    /// <returns>El rastro creado</returns>
    [HttpPost]
    public async Task<ActionResult<PropertyTraceDto>> CreateTrace(CreatePropertyTraceDto createTraceDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var trace = await _propertyTraceService.CreatePropertyTraceAsync(createTraceDto);
            return CreatedAtAction(nameof(GetTrace), new { id = trace.IdPropertyTrace }, trace);
        }
        catch (ArgumentException ex)
        {

            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating property trace");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Update an existing property trace
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<PropertyTraceDto>> UpdateTrace(int id, UpdatePropertyTraceDto updateTraceDto)
    {
        try
        {
            var trace = await _propertyTraceService.UpdatePropertyTraceAsync(id, updateTraceDto);
            if (trace == null)
                return NotFound();

            return Ok(trace);
        }
        catch (ArgumentException ex)
        {

            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating property trace with id {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Delete a property trace
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTrace(int id)
    {
        try
        {
            var result = await _propertyTraceService.DeletePropertyTraceAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting property trace with id {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}