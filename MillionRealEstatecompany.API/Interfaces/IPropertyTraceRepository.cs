using MillionRealEstatecompany.API.Models;

namespace MillionRealEstatecompany.API.Interfaces;

public interface IPropertyTraceRepository : IRepository<PropertyTrace>
{
    Task<IEnumerable<PropertyTrace>> GetTracesByPropertyAsync(int propertyId);
    Task<PropertyTrace?> GetLatestTraceByPropertyAsync(int propertyId);
}