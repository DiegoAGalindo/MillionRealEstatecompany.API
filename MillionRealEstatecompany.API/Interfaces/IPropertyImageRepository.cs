using MillionRealEstatecompany.API.Models;

namespace MillionRealEstatecompany.API.Interfaces;

public interface IPropertyImageRepository : IRepository<PropertyImage>
{
    Task<IEnumerable<PropertyImage>> GetImagesByPropertyAsync(int propertyId);
    Task<IEnumerable<PropertyImage>> GetEnabledImagesByPropertyAsync(int propertyId);
}