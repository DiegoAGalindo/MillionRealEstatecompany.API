using Microsoft.EntityFrameworkCore;
using MillionRealEstatecompany.API.Data;
using MillionRealEstatecompany.API.Interfaces;
using MillionRealEstatecompany.API.Models;

namespace MillionRealEstatecompany.API.Repositories;

public class OwnerRepository : Repository<Owner>, IOwnerRepository
{
    public OwnerRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Owner?> GetOwnerWithPropertiesAsync(int id)
    {
        return await _dbSet
            .Include(o => o.Properties)
            .FirstOrDefaultAsync(o => o.IdOwner == id);
    }

    public async Task<IEnumerable<Owner>> GetOwnersWithPropertiesAsync()
    {
        return await _dbSet
            .Include(o => o.Properties)
            .ToListAsync();
    }

    public async Task<bool> HasPropertiesAsync(int ownerId)
    {
        return await _context.Properties
            .AnyAsync(p => p.IdOwner == ownerId);
    }
}