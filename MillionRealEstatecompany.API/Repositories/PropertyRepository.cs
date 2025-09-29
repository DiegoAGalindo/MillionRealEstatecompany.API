using Microsoft.EntityFrameworkCore;
using MillionRealEstatecompany.API.Data;
using MillionRealEstatecompany.API.DTOs;
using MillionRealEstatecompany.API.Interfaces;
using MillionRealEstatecompany.API.Models;

namespace MillionRealEstatecompany.API.Repositories;

/// <summary>
/// Repositorio para las operaciones de acceso a datos de propiedades
/// </summary>
public class PropertyRepository : Repository<Property>, IPropertyRepository
{
    /// <summary>
    /// Inicializa una nueva instancia del repositorio de propiedades
    /// </summary>
    /// <param name="context">Contexto de la base de datos</param>
    public PropertyRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Obtiene una propiedad con todos sus detalles incluyendo propietario, imágenes y trazas
    /// </summary>
    /// <param name="id">Identificador de la propiedad</param>
    /// <returns>Propiedad con todos sus detalles o null si no se encuentra</returns>
    public async Task<Property?> GetPropertyWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(p => p.Owner)
            .Include(p => p.PropertyImages)
            .Include(p => p.PropertyTraces)
            .FirstOrDefaultAsync(p => p.IdProperty == id);
    }

    /// <summary>
    /// Obtiene todas las propiedades que pertenecen a un propietario específico
    /// </summary>
    /// <param name="ownerId">Identificador del propietario</param>
    /// <returns>Lista de propiedades del propietario con información del mismo</returns>
    public async Task<IEnumerable<Property>> GetPropertiesByOwnerAsync(int ownerId)
    {
        return await _dbSet
            .Where(p => p.IdOwner == ownerId)
            .Include(p => p.Owner)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene todas las propiedades con información de sus propietarios
    /// </summary>
    /// <returns>Lista de todas las propiedades con información del propietario</returns>
    public async Task<IEnumerable<Property>> GetPropertiesWithOwnerAsync()
    {
        return await _dbSet
            .Include(p => p.Owner)
            .ToListAsync();
    }

    /// <summary>
    /// Busca propiedades aplicando filtros específicos
    /// </summary>
    /// <param name="filter">Filtros de búsqueda</param>
    /// <returns>Lista de propiedades que cumplen los filtros</returns>
    public async Task<IEnumerable<Property>> SearchPropertiesAsync(PropertySearchFilter filter)
    {
        var query = _dbSet.Include(p => p.Owner).AsQueryable();

        // Filtro por precio mínimo
        if (filter.MinPrice.HasValue)
        {
            query = query.Where(p => p.Price >= filter.MinPrice.Value);
        }

        // Filtro por precio máximo
        if (filter.MaxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= filter.MaxPrice.Value);
        }

        // Filtro por año mínimo
        if (filter.MinYear.HasValue)
        {
            query = query.Where(p => p.Year >= filter.MinYear.Value);
        }

        // Filtro por año máximo
        if (filter.MaxYear.HasValue)
        {
            query = query.Where(p => p.Year <= filter.MaxYear.Value);
        }

        // Filtro por propietario
        if (filter.OwnerId.HasValue)
        {
            query = query.Where(p => p.IdOwner == filter.OwnerId.Value);
        }

        // Filtro por ciudad/dirección
        if (!string.IsNullOrWhiteSpace(filter.City))
        {
            query = query.Where(p => p.Address.Contains(filter.City));
        }

        // Filtro por nombre
        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            query = query.Where(p => p.Name.Contains(filter.Name));
        }

        return await query.ToListAsync();
    }

    /// <summary>
    /// Verifica si existe una propiedad con el código interno especificado
    /// </summary>
    /// <param name="codeInternal">Código interno a verificar</param>
    /// <param name="excludeId">ID de propiedad a excluir de la búsqueda (opcional)</param>
    /// <returns>True si existe el código, false en caso contrario</returns>
    public async Task<bool> CodeInternalExistsAsync(string codeInternal, int? excludeId = null)
    {
        var query = _dbSet.Where(p => p.CodeInternal == codeInternal);

        if (excludeId.HasValue)
        {
            query = query.Where(p => p.IdProperty != excludeId.Value);
        }

        return await query.AnyAsync();
    }
}