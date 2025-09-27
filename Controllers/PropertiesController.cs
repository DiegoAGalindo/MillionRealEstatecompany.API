using Microsoft.AspNetCore.Mvc;
using MillionRealEstatecompany.API.DTOs;
using MillionRealEstatecompany.API.Interfaces;

namespace MillionRealEstatecompany.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertiesController : ControllerBase
{
    private readonly IPropertyService _propertyService;
    private readonly ILogger<PropertiesController> _logger;

    public PropertiesController(IPropertyService propertyService, ILogger<PropertiesController> logger)
    {
        _propertyService = propertyService;
        _logger = logger;
    }

    /// <summary>
    /// Get all properties
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PropertyDto>>> GetAllProperties()
    {
        try
        {
            var properties = await _propertyService.GetAllPropertiesAsync();
            return Ok(properties);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all properties");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get property by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<PropertyDto>> GetProperty(int id)
    {
        try
        {
            var property = await _propertyService.GetPropertyByIdAsync(id);
            if (property == null)
                return NotFound();
            
            return Ok(property);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting property with id {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get property with all details (images and traces)
    /// </summary>
    [HttpGet("{id}/details")]
    public async Task<ActionResult<PropertyDetailDto>> GetPropertyWithDetails(int id)
    {
        try
        {
            var property = await _propertyService.GetPropertyWithDetailsAsync(id);
            if (property == null)
                return NotFound();
            
            return Ok(property);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting property details with id {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get properties by owner ID
    /// </summary>
    [HttpGet("by-owner/{ownerId}")]
    public async Task<ActionResult<IEnumerable<PropertyDto>>> GetPropertiesByOwner(int ownerId)
    {
        try
        {
            var properties = await _propertyService.GetPropertiesByOwnerAsync(ownerId);
            return Ok(properties);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting properties for owner {OwnerId}", ownerId);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Create a new property
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<PropertyDto>> CreateProperty(CreatePropertyDto createPropertyDto)
    {
        try
        {
            var property = await _propertyService.CreatePropertyAsync(createPropertyDto);
            return CreatedAtAction(nameof(GetProperty), new { id = property.IdProperty }, property);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument creating property");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating property");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Update an existing property
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<PropertyDto>> UpdateProperty(int id, UpdatePropertyDto updatePropertyDto)
    {
        try
        {
            var property = await _propertyService.UpdatePropertyAsync(id, updatePropertyDto);
            if (property == null)
                return NotFound();
            
            return Ok(property);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument updating property with id {Id}", id);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating property with id {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Delete a property
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProperty(int id)
    {
        try
        {
            var result = await _propertyService.DeletePropertyAsync(id);
            if (!result)
                return NotFound();
            
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting property with id {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}