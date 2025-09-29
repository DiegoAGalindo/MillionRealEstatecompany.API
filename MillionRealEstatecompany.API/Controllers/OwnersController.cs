using Microsoft.AspNetCore.Mvc;
using MillionRealEstatecompany.API.DTOs;
using MillionRealEstatecompany.API.Interfaces;

namespace MillionRealEstatecompany.API.Controllers;

/// <summary>
/// Controlador para gestionar las operaciones de propietarios
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class OwnersController : ControllerBase
{
    private readonly IOwnerService _ownerService;
    private readonly ILogger<OwnersController> _logger;

    public OwnersController(IOwnerService ownerService, ILogger<OwnersController> logger)
    {
        _ownerService = ownerService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todos los propietarios registrados en el sistema
    /// </summary>
    /// <returns>Lista de todos los propietarios</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OwnerDto>>> GetAllOwners()
    {
        try
        {
            var owners = await _ownerService.GetAllOwnersAsync();
            return Ok(owners);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all owners");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Obtiene un propietario por su identificador
    /// </summary>
    /// <param name="id">Identificador del propietario</param>
    /// <returns>Propietario encontrado</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<OwnerDto>> GetOwner(int id)
    {
        try
        {
            var owner = await _ownerService.GetOwnerByIdAsync(id);
            if (owner == null)
                return NotFound();

            return Ok(owner);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting owner with id {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Obtiene un propietario por su número de documento de identificación
    /// </summary>
    /// <param name="documentNumber">Número de documento de identificación del propietario</param>
    /// <returns>Propietario encontrado</returns>
    [HttpGet("by-document/{documentNumber}")]
    public async Task<ActionResult<OwnerDto>> GetOwnerByDocumentNumber(string documentNumber)
    {
        try
        {
            var owner = await _ownerService.GetOwnerByDocumentNumberAsync(documentNumber);
            if (owner == null)
                return NotFound(new { message = "Owner not found" });

            return Ok(owner);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid document number: {DocumentNumber}", documentNumber);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting owner with document number {DocumentNumber}", documentNumber);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Crea un nuevo propietario en el sistema
    /// </summary>
    /// <param name="createOwnerDto">Datos del propietario a crear</param>
    /// <returns>El propietario creado</returns>
    [HttpPost]
    public async Task<ActionResult<OwnerDto>> CreateOwner(CreateOwnerDto createOwnerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var owner = await _ownerService.CreateOwnerAsync(createOwnerDto);
            return CreatedAtAction(nameof(GetOwner), new { id = owner.IdOwner }, owner);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Validation error creating owner: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating owner");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Actualiza un propietario existente en el sistema
    /// </summary>
    /// <param name="id">Identificador del propietario a actualizar</param>
    /// <param name="updateOwnerDto">Nuevos datos del propietario</param>
    /// <returns>El propietario actualizado</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<OwnerDto>> UpdateOwner(int id, UpdateOwnerDto updateOwnerDto)
    {
        try
        {
            var owner = await _ownerService.UpdateOwnerAsync(id, updateOwnerDto);
            if (owner == null)
                return NotFound();

            return Ok(owner);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Validation error updating owner with id {Id}: {Message}", id, ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating owner with id {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Elimina un propietario del sistema
    /// </summary>
    /// <param name="id">Identificador del propietario a eliminar</param>
    /// <returns>NoContent si se eliminó correctamente</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOwner(int id)
    {
        try
        {
            var result = await _ownerService.DeleteOwnerAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting owner with id {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }
}