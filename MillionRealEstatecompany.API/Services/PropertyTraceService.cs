using AutoMapper;
using MillionRealEstatecompany.API.DTOs;
using MillionRealEstatecompany.API.Interfaces;
using MillionRealEstatecompany.API.Models;

namespace MillionRealEstatecompany.API.Services;

public class PropertyTraceService : IPropertyTraceService
{
    private readonly IPropertyTraceRepository _propertyTraceRepository;
    private readonly IPropertyRepository _propertyRepository;
    private readonly IMapper _mapper;

    public PropertyTraceService(IPropertyTraceRepository propertyTraceRepository, IPropertyRepository propertyRepository, IMapper mapper)
    {
        _propertyTraceRepository = propertyTraceRepository ?? throw new ArgumentNullException(nameof(propertyTraceRepository));
        _propertyRepository = propertyRepository ?? throw new ArgumentNullException(nameof(propertyRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<PropertyTraceDto>> GetTracesByPropertyIdAsync(int propertyId)
    {
        var traces = await _propertyTraceRepository.GetByIdPropertyAsync(propertyId);
        return _mapper.Map<IEnumerable<PropertyTraceDto>>(traces);
    }

    public async Task<PropertyTraceDto?> GetTraceByIdAsync(int id)
    {
        var trace = await _propertyTraceRepository.GetByIdPropertyTraceAsync(id);
        return trace != null ? _mapper.Map<PropertyTraceDto>(trace) : null;
    }

    public async Task<PropertyTraceDto> CreateTraceAsync(CreatePropertyTraceDto createDto)
    {
        // Verificar que la propiedad existe
        var property = await _propertyRepository.GetByIdPropertyAsync(createDto.IdProperty ?? 0);
        if (property == null)
        {
            throw new ArgumentException($"Property with ID {createDto.IdProperty} not found.");
        }

        var trace = _mapper.Map<PropertyTrace>(createDto);
        trace.PropertyId = property.Id!;
        
        var createdTrace = await _propertyTraceRepository.CreateAsync(trace);
        return _mapper.Map<PropertyTraceDto>(createdTrace);
    }

    public async Task<PropertyTraceDto?> UpdateTraceAsync(int id, UpdatePropertyTraceDto updateDto)
    {
        var trace = await _propertyTraceRepository.GetByIdPropertyTraceAsync(id);
        if (trace == null) return null;

        _mapper.Map(updateDto, trace);
        
        var updatedTrace = await _propertyTraceRepository.UpdateAsync(trace.Id!, trace);
        return updatedTrace != null ? _mapper.Map<PropertyTraceDto>(updatedTrace) : null;
    }

    public async Task<bool> DeleteTraceAsync(int id)
    {
        var trace = await _propertyTraceRepository.GetByIdPropertyTraceAsync(id);
        if (trace == null) return false;

        return await _propertyTraceRepository.DeleteAsync(trace.Id!);
    }

    // Métodos con nombres alternativos para compatibilidad
    public Task<IEnumerable<PropertyTraceDto>> GetTracesByPropertyAsync(int propertyId) => GetTracesByPropertyIdAsync(propertyId);
    public Task<PropertyTraceDto> CreatePropertyTraceAsync(CreatePropertyTraceDto createDto) => CreateTraceAsync(createDto);
    public Task<PropertyTraceDto?> UpdatePropertyTraceAsync(int id, UpdatePropertyTraceDto updateDto) => UpdateTraceAsync(id, updateDto);
    public Task<bool> DeletePropertyTraceAsync(int id) => DeleteTraceAsync(id);
}