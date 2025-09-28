using AutoMapper;
using MillionRealEstatecompany.API.DTOs;
using MillionRealEstatecompany.API.Interfaces;
using MillionRealEstatecompany.API.Models;

namespace MillionRealEstatecompany.API.Services;

/// <summary>
/// Service for managing property trace business logic
/// </summary>
public class PropertyTraceService : IPropertyTraceService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PropertyTraceService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PropertyTraceDto>> GetTracesByPropertyAsync(int propertyId)
    {
        var traces = await _unitOfWork.PropertyTraces.GetTracesByPropertyAsync(propertyId);
        return _mapper.Map<IEnumerable<PropertyTraceDto>>(traces);
    }

    public async Task<PropertyTraceDto?> GetTraceByIdAsync(int id)
    {
        var trace = await _unitOfWork.PropertyTraces.GetByIdAsync(id);
        return trace == null ? null : _mapper.Map<PropertyTraceDto>(trace);
    }

    public async Task<PropertyTraceDto> CreatePropertyTraceAsync(CreatePropertyTraceDto createTraceDto)
    {
        // Validar que los campos requeridos no sean null
        if (!createTraceDto.DateSale.HasValue)
            throw new ArgumentException("La fecha de venta es obligatoria");
        if (!createTraceDto.Value.HasValue)
            throw new ArgumentException("El valor de la transacción es obligatorio");
        if (!createTraceDto.Tax.HasValue)
            throw new ArgumentException("El impuesto es obligatorio");
        if (!createTraceDto.IdProperty.HasValue)
            throw new ArgumentException("El ID de la propiedad es obligatorio");

        var propertyExists = await _unitOfWork.Properties.ExistsAsync(createTraceDto.IdProperty.Value);
        if (!propertyExists)
            throw new ArgumentException("Property not found.");

        var trace = _mapper.Map<PropertyTrace>(createTraceDto);
        await _unitOfWork.PropertyTraces.AddAsync(trace);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<PropertyTraceDto>(trace);
    }

    public async Task<PropertyTraceDto?> UpdatePropertyTraceAsync(int id, UpdatePropertyTraceDto updateTraceDto)
    {
        var existingTrace = await _unitOfWork.PropertyTraces.GetByIdAsync(id);
        if (existingTrace == null)
            return null;

        var propertyExists = await _unitOfWork.Properties.ExistsAsync(updateTraceDto.IdProperty);
        if (!propertyExists)
            throw new ArgumentException("Property not found.");

        _mapper.Map(updateTraceDto, existingTrace);
        await _unitOfWork.PropertyTraces.UpdateAsync(existingTrace);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<PropertyTraceDto>(existingTrace);
    }

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