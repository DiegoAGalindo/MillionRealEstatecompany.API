using AutoMapper;
using MillionRealEstatecompany.API.DTOs;
using MillionRealEstatecompany.API.Interfaces;
using MillionRealEstatecompany.API.Models;

namespace MillionRealEstatecompany.API.Services;

/// <summary>
/// Servicio para gestionar la lógica de negocio de las trazas de propiedades
/// </summary>
public class PropertyTraceService : IPropertyTraceService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Inicializa una nueva instancia del servicio de trazas de propiedades
    /// </summary>
    /// <param name="unitOfWork">Unidad de trabajo para el acceso a datos</param>
    /// <param name="mapper">Mapeador de objetos</param>
    public PropertyTraceService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtiene todas las trazas asociadas a una propiedad específica
    /// </summary>
    /// <param name="propertyId">Identificador de la propiedad</param>
    /// <returns>Lista de trazas de la propiedad</returns>
    public async Task<IEnumerable<PropertyTraceDto>> GetTracesByPropertyAsync(int propertyId)
    {
        var traces = await _unitOfWork.PropertyTraces.GetTracesByPropertyAsync(propertyId);
        return _mapper.Map<IEnumerable<PropertyTraceDto>>(traces);
    }

    /// <summary>
    /// Obtiene una traza específica por su identificador
    /// </summary>
    /// <param name="id">Identificador de la traza</param>
    /// <returns>Traza encontrada o null si no existe</returns>
    public async Task<PropertyTraceDto?> GetTraceByIdAsync(int id)
    {
        var trace = await _unitOfWork.PropertyTraces.GetByIdAsync(id);
        return trace == null ? null : _mapper.Map<PropertyTraceDto>(trace);
    }

    /// <summary>
    /// Crea una nueva traza para una propiedad
    /// </summary>
    /// <param name="createTraceDto">Datos de la traza a crear</param>
    /// <returns>La traza creada</returns>
    /// <exception cref="ArgumentException">Se lanza cuando los datos de entrada son inválidos</exception>
    public async Task<PropertyTraceDto> CreatePropertyTraceAsync(CreatePropertyTraceDto createTraceDto)
    {
        if (!createTraceDto.DateSale.HasValue)
            throw new ArgumentException("Sale date is required");
        if (!createTraceDto.Value.HasValue)
            throw new ArgumentException("Transaction value is required");
        if (!createTraceDto.Tax.HasValue)
            throw new ArgumentException("Tax is required");
        if (!createTraceDto.IdProperty.HasValue)
            throw new ArgumentException("Property ID is required");

        var propertyExists = await _unitOfWork.Properties.ExistsAsync(createTraceDto.IdProperty.Value);
        if (!propertyExists)
            throw new ArgumentException("Property not found");

        var trace = _mapper.Map<PropertyTrace>(createTraceDto);
        await _unitOfWork.PropertyTraces.AddAsync(trace);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<PropertyTraceDto>(trace);
    }

    /// <summary>
    /// Actualiza una traza existente de una propiedad
    /// </summary>
    /// <param name="id">Identificador de la traza a actualizar</param>
    /// <param name="updateTraceDto">Nuevos datos de la traza</param>
    /// <returns>La traza actualizada o null si no se encuentra</returns>
    /// <exception cref="ArgumentException">Se lanza cuando la propiedad no existe</exception>
    public async Task<PropertyTraceDto?> UpdatePropertyTraceAsync(int id, UpdatePropertyTraceDto updateTraceDto)
    {
        var existingTrace = await _unitOfWork.PropertyTraces.GetByIdAsync(id);
        if (existingTrace == null)
            return null;

        var propertyExists = await _unitOfWork.Properties.ExistsAsync(updateTraceDto.IdProperty);
        if (!propertyExists)
            throw new ArgumentException("Property not found");

        _mapper.Map(updateTraceDto, existingTrace);
        await _unitOfWork.PropertyTraces.UpdateAsync(existingTrace);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<PropertyTraceDto>(existingTrace);
    }

    /// <summary>
    /// Elimina una traza de una propiedad
    /// </summary>
    /// <param name="id">Identificador de la traza a eliminar</param>
    /// <returns>True si se eliminó correctamente, false si no se encontró</returns>
    public async Task<bool> DeletePropertyTraceAsync(int id)
    {
        var trace = await _unitOfWork.PropertyTraces.GetByIdAsync(id);
        if (trace == null)
            return false;

        await _unitOfWork.PropertyTraces.DeleteAsync(trace);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}