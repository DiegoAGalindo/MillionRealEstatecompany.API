using Microsoft.EntityFrameworkCore;
using MillionRealEstatecompany.API.Data;
using MillionRealEstatecompany.API.Interfaces;
using MillionRealEstatecompany.API.Models;

namespace MillionRealEstatecompany.API.Repositories;

public class PropertyTraceRepository : Repository<PropertyTrace>, IPropertyTraceRepository
{
    public PropertyTraceRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<PropertyTrace>> GetTracesByPropertyAsync(int propertyId)
    {
        return await _dbSet
            .Where(pt => pt.IdProperty == propertyId)
            .OrderByDescending(pt => pt.DateSale)
            .ToListAsync();
    }

    public async Task<PropertyTrace?> GetLatestTraceByPropertyAsync(int propertyId)
    {
        return await _dbSet
            .Where(pt => pt.IdProperty == propertyId)
            .OrderByDescending(pt => pt.DateSale)
            .FirstOrDefaultAsync();
    }
}