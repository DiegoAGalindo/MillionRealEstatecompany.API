using MongoDB.Driver;
using MongoDB.Bson;
using MillionRealEstatecompany.API.Data;
using MillionRealEstatecompany.API.Interfaces;
using MillionRealEstatecompany.API.Models;

namespace MillionRealEstatecompany.API.Repositories;

/// <summary>
/// Repositorio MongoDB para operaciones con Owner
/// </summary>
public class OwnerRepository : IOwnerRepository
{
    private readonly IMongoCollection<Owner> _owners;

    public OwnerRepository(MongoDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        _owners = context.Owners;
    }

    public async Task<IEnumerable<Owner>> GetAllAsync()
    {
        return await _owners.Find(_ => true).ToListAsync();
    }

    public async Task<Owner?> GetByIdAsync(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            return null;

        return await _owners.Find(o => o.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Owner?> GetByIdOwnerAsync(int idOwner)
    {
        return await _owners.Find(o => o.IdOwner == idOwner).FirstOrDefaultAsync();
    }

    public async Task<Owner> CreateAsync(Owner owner)
    {
        owner.IdOwner = await GetNextIdOwnerAsync();
        owner.CreatedAt = DateTime.UtcNow;
        owner.UpdatedAt = DateTime.UtcNow;
        
        await _owners.InsertOneAsync(owner);
        return owner;
    }

    public async Task<Owner?> UpdateAsync(string id, Owner owner)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            return null;

        owner.UpdatedAt = DateTime.UtcNow;
        
        var result = await _owners.ReplaceOneAsync(o => o.Id == id, owner);
        return result.MatchedCount > 0 ? owner : null;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            return false;

        var result = await _owners.DeleteOneAsync(o => o.Id == id);
        return result.DeletedCount > 0;
    }

    public async Task<bool> DocumentNumberExistsAsync(string documentNumber, string? excludeId = null)
    {
        var filter = Builders<Owner>.Filter.Eq(o => o.DocumentNumber, documentNumber);
        
        if (!string.IsNullOrEmpty(excludeId))
        {
            filter = Builders<Owner>.Filter.And(filter, 
                Builders<Owner>.Filter.Ne(o => o.Id, excludeId));
        }

        var count = await _owners.CountDocumentsAsync(filter);
        return count > 0;
    }

    public async Task<bool> EmailExistsAsync(string email, string? excludeId = null)
    {
        var filter = Builders<Owner>.Filter.Eq(o => o.Email, email);
        
        if (!string.IsNullOrEmpty(excludeId))
        {
            filter = Builders<Owner>.Filter.And(filter, 
                Builders<Owner>.Filter.Ne(o => o.Id, excludeId));
        }

        var count = await _owners.CountDocumentsAsync(filter);
        return count > 0;
    }

    public async Task<Owner?> GetByEmailAsync(string email)
    {
        return await _owners.Find(o => o.Email == email).FirstOrDefaultAsync();
    }

    public async Task<Owner?> GetByDocumentNumberAsync(string documentNumber)
    {
        return await _owners.Find(o => o.DocumentNumber == documentNumber).FirstOrDefaultAsync();
    }

    public async Task<int> GetNextIdOwnerAsync()
    {
        var lastOwner = await _owners
            .Find(_ => true)
            .SortByDescending(o => o.IdOwner)
            .FirstOrDefaultAsync();

        return lastOwner?.IdOwner + 1 ?? 1;
    }

    public async Task UpdatePropertiesCountAsync(int idOwner, int count)
    {
        var filter = Builders<Owner>.Filter.Eq(o => o.IdOwner, idOwner);
        var update = Builders<Owner>.Update
            .Set(o => o.PropertiesCount, count)
            .Set(o => o.UpdatedAt, DateTime.UtcNow);

        await _owners.UpdateOneAsync(filter, update);
    }
}