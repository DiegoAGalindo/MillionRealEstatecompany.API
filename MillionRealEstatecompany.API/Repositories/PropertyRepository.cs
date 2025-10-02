using MongoDB.Driver;
using MongoDB.Bson;
using MillionRealEstatecompany.API.Data;
using MillionRealEstatecompany.API.Interfaces;
using MillionRealEstatecompany.API.Models;
using MillionRealEstatecompany.API.DTOs;

namespace MillionRealEstatecompany.API.Repositories;

/// <summary>
/// Repositorio MongoDB para operaciones con Property
/// </summary>
public class PropertyRepository : IPropertyRepository
{
    private readonly IMongoCollection<Property> _properties;

    public PropertyRepository(MongoDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        _properties = context.Properties;
    }

    public async Task<IEnumerable<Property>> GetAllAsync()
    {
        return await _properties.Find(_ => true).ToListAsync();
    }

    public async Task<Property?> GetByIdAsync(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            return null;

        return await _properties.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Property?> GetByIdPropertyAsync(int idProperty)
    {
        return await _properties.Find(p => p.IdProperty == idProperty).FirstOrDefaultAsync();
    }

    public async Task<Property> CreateAsync(Property property)
    {
        property.IdProperty = await GetNextIdPropertyAsync();
        property.CreatedAt = DateTime.UtcNow;
        property.UpdatedAt = DateTime.UtcNow;
        
        await _properties.InsertOneAsync(property);
        return property;
    }

    public async Task<Property?> UpdateAsync(string id, Property property)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            return null;

        property.UpdatedAt = DateTime.UtcNow;
        
        var result = await _properties.ReplaceOneAsync(p => p.Id == id, property);
        return result.MatchedCount > 0 ? property : null;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            return false;

        var result = await _properties.DeleteOneAsync(p => p.Id == id);
        return result.DeletedCount > 0;
    }

    public async Task<IEnumerable<Property>> GetPropertiesByOwnerAsync(int ownerId)
    {
        return await _properties.Find(p => p.Owner.IdOwner == ownerId).ToListAsync();
    }

    public async Task<IEnumerable<Property>> SearchPropertiesAsync(PropertySearchFilter filter)
    {
        var filterBuilder = Builders<Property>.Filter;
        var filters = new List<FilterDefinition<Property>>();

        // Filtro por propietario
        if (filter.OwnerId.HasValue)
        {
            filters.Add(filterBuilder.Eq("owner.idOwner", filter.OwnerId.Value));
        }

        // Filtro por rango de precio
        if (filter.MinPrice.HasValue)
        {
            filters.Add(filterBuilder.Gte(p => p.Price, filter.MinPrice.Value));
        }
        
        if (filter.MaxPrice.HasValue)
        {
            filters.Add(filterBuilder.Lte(p => p.Price, filter.MaxPrice.Value));
        }

        // Filtro por año
        if (filter.MinYear.HasValue)
        {
            filters.Add(filterBuilder.Gte(p => p.Year, filter.MinYear.Value));
        }
        
        if (filter.MaxYear.HasValue)
        {
            filters.Add(filterBuilder.Lte(p => p.Year, filter.MaxYear.Value));
        }

        // Filtro por nombre (búsqueda de texto)
        if (!string.IsNullOrEmpty(filter.Name))
        {
            filters.Add(filterBuilder.Regex(p => p.Name, new BsonRegularExpression(filter.Name, "i")));
        }

        // Filtro por ciudad (búsqueda de texto en dirección)
        if (!string.IsNullOrEmpty(filter.City))
        {
            filters.Add(filterBuilder.Regex(p => p.Address, new BsonRegularExpression(filter.City, "i")));
        }

        // Combinar todos los filtros
        var combinedFilter = filters.Count > 0 
            ? filterBuilder.And(filters) 
            : filterBuilder.Empty;

        return await _properties.Find(combinedFilter).ToListAsync();
    }

    public async Task<bool> CodeInternalExistsAsync(string codeInternal, string? excludeId = null)
    {
        var filter = Builders<Property>.Filter.Eq(p => p.CodeInternal, codeInternal);
        
        if (!string.IsNullOrEmpty(excludeId))
        {
            filter = Builders<Property>.Filter.And(filter, 
                Builders<Property>.Filter.Ne(p => p.Id, excludeId));
        }

        var count = await _properties.CountDocumentsAsync(filter);
        return count > 0;
    }

    public async Task<int> GetNextIdPropertyAsync()
    {
        var lastProperty = await _properties
            .Find(_ => true)
            .SortByDescending(p => p.IdProperty)
            .FirstOrDefaultAsync();

        return lastProperty?.IdProperty + 1 ?? 1;
    }
}