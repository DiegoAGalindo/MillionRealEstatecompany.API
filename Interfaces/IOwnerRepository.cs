using MillionRealEstatecompany.API.Models;

namespace MillionRealEstatecompany.API.Interfaces;

public interface IOwnerRepository : IRepository<Owner>
{
    Task<Owner?> GetOwnerWithPropertiesAsync(int id);
    Task<IEnumerable<Owner>> GetOwnersWithPropertiesAsync();
    Task<bool> HasPropertiesAsync(int ownerId);
}