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

    // Método de validación optimizado con índice único
    Task<bool> DocumentNumberExistsAsync(string documentNumber);

    // Métodos de búsqueda optimizados con índices
    Task<Owner?> GetByEmailAsync(string email); // Email indexado para búsquedas rápidas
    Task<Owner?> GetByDocumentNumberAsync(string documentNumber);
}