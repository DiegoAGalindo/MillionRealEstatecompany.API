using AutoMapper;
using MillionRealEstatecompany.API.DTOs;
using MillionRealEstatecompany.API.Interfaces;
using MillionRealEstatecompany.API.Models;

namespace MillionRealEstatecompany.API.Services;

/// <summary>
/// Servicio para gestionar la lógica de negocio de las propiedades inmobiliarias
/// </summary>
public class PropertyService : IPropertyService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructor del servicio de propiedades
    /// </summary>
    /// <param name="unitOfWork">Unidad de trabajo para acceso a datos</param>
    /// <param name="mapper">Mapeador de objetos</param>
    public PropertyService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtiene todas las propiedades con información del propietario
    /// </summary>
    /// <returns>Lista de propiedades</returns>
    public async Task<IEnumerable<PropertyDto>> GetAllPropertiesAsync()
    {
        var properties = await _unitOfWork.Properties.GetPropertiesWithOwnerAsync();
        return _mapper.Map<IEnumerable<PropertyDto>>(properties);
    }

    /// <summary>
    /// Obtiene una propiedad por su identificador
    /// </summary>
    /// <param name="id">Identificador de la propiedad</param>
    /// <returns>Propiedad encontrada o null si no existe</returns>
    public async Task<PropertyDto?> GetPropertyByIdAsync(int id)
    {
        var property = await _unitOfWork.Properties.GetPropertyWithDetailsAsync(id);
        return property == null ? null : _mapper.Map<PropertyDto>(property);
    }

    /// <summary>
    /// Obtiene una propiedad con todos sus detalles (imágenes y trazas)
    /// </summary>
    /// <param name="id">Identificador de la propiedad</param>
    /// <returns>Propiedad con detalles completos o null si no existe</returns>
    public async Task<PropertyDetailDto?> GetPropertyWithDetailsAsync(int id)
    {
        var property = await _unitOfWork.Properties.GetPropertyWithDetailsAsync(id);
        return property == null ? null : _mapper.Map<PropertyDetailDto>(property);
    }

    /// <summary>
    /// Obtiene todas las propiedades de un propietario específico
    /// </summary>
    /// <param name="ownerId">Identificador del propietario</param>
    /// <returns>Lista de propiedades del propietario</returns>
    public async Task<IEnumerable<PropertyDto>> GetPropertiesByOwnerAsync(int ownerId)
    {
        var properties = await _unitOfWork.Properties.GetPropertiesByOwnerAsync(ownerId);
        return _mapper.Map<IEnumerable<PropertyDto>>(properties);
    }

    /// <summary>
    /// Crea una nueva propiedad validando que el propietario exista y el código interno sea único
    /// </summary>
    /// <param name="createPropertyDto">Datos de la propiedad a crear</param>
    /// <returns>DTO de la propiedad creada con información del propietario</returns>
    /// <exception cref="ArgumentException">Se lanza cuando el propietario no existe o el código interno ya está en uso</exception>
    public async Task<PropertyDto> CreatePropertyAsync(CreatePropertyDto createPropertyDto)
    {
        if (!createPropertyDto.Price.HasValue)
            throw new ArgumentException("Price is required");
        if (!createPropertyDto.Year.HasValue)
            throw new ArgumentException("Construction year is required");
        if (!createPropertyDto.IdOwner.HasValue)
            throw new ArgumentException("Owner ID is required");

        var ownerExists = await _unitOfWork.Owners.ExistsAsync(createPropertyDto.IdOwner.Value);
        if (!ownerExists)
            throw new ArgumentException("Owner not found");

        var codeExists = await _unitOfWork.Properties.CodeInternalExistsAsync(createPropertyDto.CodeInternal);
        if (codeExists)
            throw new ArgumentException("Internal code already exists");

        var property = _mapper.Map<Property>(createPropertyDto);
        await _unitOfWork.Properties.AddAsync(property);
        await _unitOfWork.SaveChangesAsync();

        var createdProperty = await _unitOfWork.Properties.GetPropertyWithDetailsAsync(property.IdProperty);
        return _mapper.Map<PropertyDto>(createdProperty);
    }

    /// <summary>
    /// Actualiza una propiedad existente validando propietario y unicidad del código interno
    /// </summary>
    /// <param name="id">Identificador de la propiedad a actualizar</param>
    /// <param name="updatePropertyDto">Datos actualizados de la propiedad</param>
    /// <returns>DTO de la propiedad actualizada, null si no existe</returns>
    /// <exception cref="ArgumentException">Se lanza cuando el propietario no existe o el código interno ya está en uso</exception>
    public async Task<PropertyDto?> UpdatePropertyAsync(int id, UpdatePropertyDto updatePropertyDto)
    {
        var existingProperty = await _unitOfWork.Properties.GetByIdAsync(id);
        if (existingProperty == null)
            return null;

        var ownerExists = await _unitOfWork.Owners.ExistsAsync(updatePropertyDto.IdOwner);
        if (!ownerExists)
            throw new ArgumentException("Owner not found");

        var codeExists = await _unitOfWork.Properties.CodeInternalExistsAsync(updatePropertyDto.CodeInternal, id);
        if (codeExists)
            throw new ArgumentException("Internal code already exists");

        _mapper.Map(updatePropertyDto, existingProperty);
        await _unitOfWork.Properties.UpdateAsync(existingProperty);
        await _unitOfWork.SaveChangesAsync();

        var updatedProperty = await _unitOfWork.Properties.GetPropertyWithDetailsAsync(id);
        return _mapper.Map<PropertyDto>(updatedProperty);
    }

    /// <summary>
    /// Elimina una propiedad del sistema
    /// </summary>
    /// <param name="id">Identificador de la propiedad a eliminar</param>
    /// <returns>True si se eliminó exitosamente, False si no existe</returns>
    public async Task<bool> DeletePropertyAsync(int id)
    {
        var property = await _unitOfWork.Properties.GetByIdAsync(id);
        if (property == null)
            return false;

        await _unitOfWork.Properties.DeleteAsync(property);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Verifica si existe una propiedad con el identificador especificado
    /// </summary>
    /// <param name="id">Identificador de la propiedad</param>
    /// <returns>True si existe, False en caso contrario</returns>
    public async Task<bool> PropertyExistsAsync(int id)
    {
        return await _unitOfWork.Properties.ExistsAsync(id);
    }
}