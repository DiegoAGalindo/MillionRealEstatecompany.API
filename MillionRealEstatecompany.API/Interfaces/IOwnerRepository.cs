using MillionRealEstatecompany.API.Models;

namespace MillionRealEstatecompany.API.Interfaces;

/// <summary>
/// Interface for owner repository operations with MongoDB
/// </summary>
public interface IOwnerRepository
{
    Task<IEnumerable<Owner>> GetAllAsync();
    Task<Owner?> GetByIdAsync(string id);
    Task<Owner?> GetByIdOwnerAsync(int idOwner);
    Task<Owner> CreateAsync(Owner owner);
    Task<Owner?> UpdateAsync(string id, Owner owner);
    Task<bool> DeleteAsync(string id);
    Task<bool> DocumentNumberExistsAsync(string documentNumber, string? excludeId = null);
    Task<bool> EmailExistsAsync(string email, string? excludeId = null);
    Task<Owner?> GetByEmailAsync(string email);
    Task<Owner?> GetByDocumentNumberAsync(string documentNumber);
    Task<int> GetNextIdOwnerAsync();
    Task UpdatePropertiesCountAsync(int idOwner, int count);
}