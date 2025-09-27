using Microsoft.AspNetCore.Mvc;
using MillionRealEstatecompany.API.DTOs;
using MillionRealEstatecompany.API.Interfaces;

namespace MillionRealEstatecompany.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertyImagesController : ControllerBase
{
    private readonly IPropertyImageService _propertyImageService;
    private readonly ILogger<PropertyImagesController> _logger;

    public PropertyImagesController(IPropertyImageService propertyImageService, ILogger<PropertyImagesController> logger)
    {
        _propertyImageService = propertyImageService;
        _logger = logger;
    }

    /// <summary>
    /// Get images by property ID
    /// </summary>
    [HttpGet("by-property/{propertyId}")]
    public async Task<ActionResult<IEnumerable<PropertyImageDto>>> GetImagesByProperty(int propertyId)
    {
        try
        {
            var images = await _propertyImageService.GetImagesByPropertyAsync(propertyId);
            return Ok(images);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting images for property {PropertyId}", propertyId);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get property image by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<PropertyImageDto>> GetImage(int id)
    {
        try
        {
            var image = await _propertyImageService.GetImageByIdAsync(id);
            if (image == null)
                return NotFound();
            
            return Ok(image);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting image with id {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Create a new property image
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<PropertyImageDto>> CreateImage(CreatePropertyImageDto createImageDto)
    {
        try
        {
            var image = await _propertyImageService.CreatePropertyImageAsync(createImageDto);
            return CreatedAtAction(nameof(GetImage), new { id = image.IdPropertyImage }, image);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument creating property image");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating property image");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Update an existing property image
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<PropertyImageDto>> UpdateImage(int id, UpdatePropertyImageDto updateImageDto)
    {
        try
        {
            var image = await _propertyImageService.UpdatePropertyImageAsync(id, updateImageDto);
            if (image == null)
                return NotFound();
            
            return Ok(image);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating property image with id {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Delete a property image
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteImage(int id)
    {
        try
        {
            var result = await _propertyImageService.DeletePropertyImageAsync(id);
            if (!result)
                return NotFound();
            
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting property image with id {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}