using MillionRealEstatecompany.API.Models;

namespace MillionRealEstatecompany.API.Interfaces;

/// <summary>
/// Interface for owner repository operations
/// </summary>
public interface IOwnerRepository : IRepository<Owner>
{
    Task<Owner?> GetOwnerWithPropertiesAsync(int id);
    Task<IEnumerable<Owner>> GetOwnersWithPropertiesAsync();
    Task<bool> HasPropertiesAsync(int ownerId);
}