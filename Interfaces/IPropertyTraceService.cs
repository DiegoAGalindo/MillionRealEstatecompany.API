using MillionRealEstatecompany.API.DTOs;

namespace MillionRealEstatecompany.API.Interfaces;

public interface IPropertyTraceService
{
    Task<IEnumerable<PropertyTraceDto>> GetTracesByPropertyAsync(int propertyId);
    Task<PropertyTraceDto?> GetTraceByIdAsync(int id);
    Task<PropertyTraceDto> CreatePropertyTraceAsync(CreatePropertyTraceDto createTraceDto);
    Task<PropertyTraceDto?> UpdatePropertyTraceAsync(int id, UpdatePropertyTraceDto updateTraceDto);
    Task<bool> DeletePropertyTraceAsync(int id);
}