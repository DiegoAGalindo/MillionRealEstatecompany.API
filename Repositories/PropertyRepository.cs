using Microsoft.EntityFrameworkCore;
using MillionRealEstatecompany.API.Data;
using MillionRealEstatecompany.API.Interfaces;
using MillionRealEstatecompany.API.Models;

namespace MillionRealEstatecompany.API.Repositories;

/// <summary>
/// Repository for property data access operations
/// </summary>
public class PropertyRepository : Repository<Property>, IPropertyRepository
{
    public PropertyRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Property?> GetPropertyWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(p => p.Owner)
            .Include(p => p.PropertyImages)
            .Include(p => p.PropertyTraces)
            .FirstOrDefaultAsync(p => p.IdProperty == id);
    }

    public async Task<IEnumerable<Property>> GetPropertiesByOwnerAsync(int ownerId)
    {
        return await _dbSet
            .Where(p => p.IdOwner == ownerId)
            .Include(p => p.Owner)
            .ToListAsync();
    }

    public async Task<IEnumerable<Property>> GetPropertiesWithOwnerAsync()
    {
        return await _dbSet
            .Include(p => p.Owner)
            .ToListAsync();
    }

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