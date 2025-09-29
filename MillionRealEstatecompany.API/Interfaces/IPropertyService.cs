using MillionRealEstatecompany.API.DTOs;

namespace MillionRealEstatecompany.API.Interfaces;

/// <summary>
/// Interfaz para las operaciones del servicio de propiedades inmobiliarias
/// </summary>
public interface IPropertyService
{
    Task<IEnumerable<PropertyDto>> GetAllPropertiesAsync();
    Task<PropertyDto?> GetPropertyByIdAsync(int id);
    Task<PropertyDetailDto?> GetPropertyWithDetailsAsync(int id);
    Task<IEnumerable<PropertyDto>> GetPropertiesByOwnerAsync(int ownerId);
    Task<PropertyDto> CreatePropertyAsync(CreatePropertyDto createPropertyDto);
    Task<PropertyDto?> UpdatePropertyAsync(int id, UpdatePropertyDto updatePropertyDto);
    Task<PropertyDto?> UpdatePropertyPriceAsync(int id, decimal newPrice);
    Task<IEnumerable<PropertyDto>> SearchPropertiesAsync(PropertySearchFilter filter);
    Task<bool> DeletePropertyAsync(int id);
    Task<bool> PropertyExistsAsync(int id);
}