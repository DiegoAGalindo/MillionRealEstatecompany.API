using Microsoft.AspNetCore.Mvc;
using MillionRealEstatecompany.API.DTOs;
using MillionRealEstatecompany.API.Interfaces;

namespace MillionRealEstatecompany.API.Controllers;

/// <summary>
/// Controlador para gestionar las operaciones de propiedades inmobiliarias
/// </summary>
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
    /// Obtiene todas las propiedades registradas en el sistema
    /// </summary>
    /// <returns>Lista de todas las propiedades con información del propietario</returns>
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
    /// Obtiene una propiedad específica por su identificador
    /// </summary>
    /// <param name="id">Identificador único de la propiedad</param>
    /// <returns>Datos de la propiedad solicitada</returns>
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
    /// Obtiene una propiedad con todos sus detalles incluyendo imágenes y trazas históricas
    /// </summary>
    /// <param name="id">Identificador único de la propiedad</param>
    /// <returns>Datos completos de la propiedad con imágenes y trazas</returns>
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
    /// Obtiene todas las propiedades que pertenecen a un propietario específico
    /// </summary>
    /// <param name="ownerId">Identificador del propietario</param>
    /// <returns>Lista de propiedades del propietario especificado</returns>
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
    /// Crea una nueva propiedad en el sistema
    /// </summary>
    /// <param name="createPropertyDto">Datos de la propiedad a crear</param>
    /// <returns>Datos de la propiedad creada</returns>
    [HttpPost]
    public async Task<ActionResult<PropertyDto>> CreateProperty(CreatePropertyDto createPropertyDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var property = await _propertyService.CreatePropertyAsync(createPropertyDto);
            return CreatedAtAction(nameof(GetProperty), new { id = property.IdProperty }, property);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating property");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Actualiza una propiedad existente
    /// </summary>
    /// <param name="id">Identificador de la propiedad a actualizar</param>
    /// <param name="updatePropertyDto">Datos actualizados de la propiedad</param>
    /// <returns>Datos de la propiedad actualizada</returns>
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
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating property with id {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Elimina una propiedad del sistema
    /// </summary>
    /// <param name="id">Identificador de la propiedad a eliminar</param>
    /// <returns>Resultado de la operación de eliminación</returns>
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