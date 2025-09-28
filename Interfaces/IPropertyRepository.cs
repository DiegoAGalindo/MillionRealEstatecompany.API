using MillionRealEstatecompany.API.Models;

namespace MillionRealEstatecompany.API.Interfaces;

/// <summary>
/// Interface for property repository operations
/// </summary>
public interface IPropertyRepository : IRepository<Property>
{
    Task<Property?> GetPropertyWithDetailsAsync(int id);
    Task<IEnumerable<Property>> GetPropertiesByOwnerAsync(int ownerId);
    Task<IEnumerable<Property>> GetPropertiesWithOwnerAsync();
    Task<bool> CodeInternalExistsAsync(string codeInternal, int? excludeId = null);
}