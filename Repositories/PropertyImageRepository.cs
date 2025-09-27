using Microsoft.EntityFrameworkCore;
using MillionRealEstatecompany.API.Data;
using MillionRealEstatecompany.API.Interfaces;
using MillionRealEstatecompany.API.Models;

namespace MillionRealEstatecompany.API.Repositories;

public class PropertyImageRepository : Repository<PropertyImage>, IPropertyImageRepository
{
    public PropertyImageRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<PropertyImage>> GetImagesByPropertyAsync(int propertyId)
    {
        return await _dbSet
            .Where(pi => pi.IdProperty == propertyId)
            .ToListAsync();
    }

    public async Task<IEnumerable<PropertyImage>> GetEnabledImagesByPropertyAsync(int propertyId)
    {
        return await _dbSet
            .Where(pi => pi.IdProperty == propertyId && pi.Enabled)
            .ToListAsync();
    }
}