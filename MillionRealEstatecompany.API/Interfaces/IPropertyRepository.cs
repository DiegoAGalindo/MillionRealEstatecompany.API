using MillionRealEstatecompany.API.DTOs;
using MillionRealEstatecompany.API.Models;

namespace MillionRealEstatecompany.API.Interfaces;

/// <summary>
/// Interface for property repository operations with MongoDB
/// </summary>
public interface IPropertyRepository
{
    Task<IEnumerable<Property>> GetAllAsync();
    Task<Property?> GetByIdAsync(string id);
    Task<Property?> GetByIdPropertyAsync(int idProperty);
    Task<Property> CreateAsync(Property property);
    Task<Property?> UpdateAsync(string id, Property property);
    Task<bool> DeleteAsync(string id);
    Task<IEnumerable<Property>> GetPropertiesByOwnerAsync(int ownerId);
    Task<IEnumerable<Property>> SearchPropertiesAsync(PropertySearchFilter filter);
    Task<bool> CodeInternalExistsAsync(string codeInternal, string? excludeId = null);
    Task<int> GetNextIdPropertyAsync();
}