using Microsoft.EntityFrameworkCore;
using MillionRealEstatecompany.API.Data;
using MillionRealEstatecompany.API.Interfaces;
using MillionRealEstatecompany.API.Models;

namespace MillionRealEstatecompany.API.Repositories;

/// <summary>
/// Repositorio para las operaciones de acceso a datos de propietarios
/// </summary>
public class OwnerRepository : Repository<Owner>, IOwnerRepository
{
    /// <summary>
    /// Inicializa una nueva instancia del repositorio de propietarios
    /// </summary>
    /// <param name="context">Contexto de la base de datos</param>
    public OwnerRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Obtiene un propietario con sus propiedades asociadas
    /// </summary>
    /// <param name="id">Identificador del propietario</param>
    /// <returns>Propietario con sus propiedades o null si no se encuentra</returns>
    public async Task<Owner?> GetOwnerWithPropertiesAsync(int id)
    {
        return await _dbSet
            .Include(o => o.Properties)
            .FirstOrDefaultAsync(o => o.IdOwner == id);
    }

    /// <summary>
    /// Obtiene todos los propietarios con sus propiedades asociadas
    /// </summary>
    /// <returns>Lista de propietarios con sus propiedades</returns>
    public async Task<IEnumerable<Owner>> GetOwnersWithPropertiesAsync()
    {
        return await _dbSet
            .Include(o => o.Properties)
            .ToListAsync();
    }

    /// <summary>
    /// Verifica si un propietario tiene propiedades asociadas
    /// </summary>
    /// <param name="ownerId">Identificador del propietario</param>
    /// <returns>True si tiene propiedades, false en caso contrario</returns>
    public async Task<bool> HasPropertiesAsync(int ownerId)
    {
        return await _context.Properties
            .AnyAsync(p => p.IdOwner == ownerId);
    }

    /// <summary>
    /// Verifica si existe un propietario con el número de documento especificado
    /// </summary>
    /// <param name="documentNumber">Número de documento a verificar</param>
    /// <returns>True si existe, false en caso contrario</returns>
    public async Task<bool> DocumentNumberExistsAsync(string documentNumber)
    {
        return await _context.Owners
            .AnyAsync(o => o.DocumentNumber == documentNumber);
    }

    /// <summary>
    /// Obtiene un propietario por su dirección de correo electrónico
    /// </summary>
    /// <param name="email">Dirección de correo electrónico</param>
    /// <returns>Propietario encontrado o null si no existe</returns>
    public async Task<Owner?> GetByEmailAsync(string email)
    {
        return await _context.Owners
            .FirstOrDefaultAsync(o => o.Email == email);
    }

    /// <summary>
    /// Obtiene un propietario por su número de documento
    /// </summary>
    /// <param name="documentNumber">Número de documento del propietario</param>
    /// <returns>Propietario encontrado o null si no existe</returns>
    public async Task<Owner?> GetByDocumentNumberAsync(string documentNumber)
    {
        return await _context.Owners
            .FirstOrDefaultAsync(o => o.DocumentNumber == documentNumber);
    }
}