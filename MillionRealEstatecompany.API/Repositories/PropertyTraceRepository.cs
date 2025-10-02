using MongoDB.Driver;
using MongoDB.Bson;
using MillionRealEstatecompany.API.Data;
using MillionRealEstatecompany.API.Interfaces;
using MillionRealEstatecompany.API.Models;

namespace MillionRealEstatecompany.API.Repositories;

/// <summary>
/// Repositorio MongoDB para operaciones con PropertyTrace
/// </summary>
public class PropertyTraceRepository : IPropertyTraceRepository
{
    private readonly IMongoCollection<PropertyTrace> _propertyTraces;

    public PropertyTraceRepository(MongoDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        _propertyTraces = context.PropertyTraces;
    }

    public async Task<IEnumerable<PropertyTrace>> GetAllAsync()
    {
        return await _propertyTraces.Find(_ => true).ToListAsync();
    }

    public async Task<PropertyTrace?> GetByIdAsync(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            return null;

        return await _propertyTraces.Find(pt => pt.Id == id).FirstOrDefaultAsync();
    }

    public async Task<PropertyTrace?> GetByIdPropertyTraceAsync(int idPropertyTrace)
    {
        return await _propertyTraces.Find(pt => pt.IdPropertyTrace == idPropertyTrace).FirstOrDefaultAsync();
    }

    public async Task<PropertyTrace> CreateAsync(PropertyTrace propertyTrace)
    {
        propertyTrace.IdPropertyTrace = await GetNextIdPropertyTraceAsync();
        propertyTrace.CreatedAt = DateTime.UtcNow;
        
        await _propertyTraces.InsertOneAsync(propertyTrace);
        return propertyTrace;
    }

    public async Task<PropertyTrace?> UpdateAsync(string id, PropertyTrace propertyTrace)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            return null;
        
        var result = await _propertyTraces.ReplaceOneAsync(pt => pt.Id == id, propertyTrace);
        return result.MatchedCount > 0 ? propertyTrace : null;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            return false;

        var result = await _propertyTraces.DeleteOneAsync(pt => pt.Id == id);
        return result.DeletedCount > 0;
    }

    public async Task<IEnumerable<PropertyTrace>> GetByPropertyIdAsync(string propertyId)
    {
        return await _propertyTraces
            .Find(pt => pt.PropertyId == propertyId)
            .SortByDescending(pt => pt.DateSale)
            .ToListAsync();
    }

    public async Task<IEnumerable<PropertyTrace>> GetByIdPropertyAsync(int idProperty)
    {
        return await _propertyTraces
            .Find(pt => pt.IdProperty == idProperty)
            .SortByDescending(pt => pt.DateSale)
            .ToListAsync();
    }

    public async Task<PropertyTrace?> GetLatestTraceByPropertyAsync(int idProperty)
    {
        return await _propertyTraces
            .Find(pt => pt.IdProperty == idProperty)
            .SortByDescending(pt => pt.DateSale)
            .FirstOrDefaultAsync();
    }

    public async Task<int> GetNextIdPropertyTraceAsync()
    {
        var lastTrace = await _propertyTraces
            .Find(_ => true)
            .SortByDescending(pt => pt.IdPropertyTrace)
            .FirstOrDefaultAsync();

        return lastTrace?.IdPropertyTrace + 1 ?? 1;
    }

    public async Task<PropertyTraceStatistics> GetStatisticsAsync(string propertyId)
    {
        var traces = await _propertyTraces
            .Find(pt => pt.PropertyId == propertyId)
            .ToListAsync();

        if (!traces.Any())
        {
            return new PropertyTraceStatistics();
        }

        return new PropertyTraceStatistics
        {
            TotalTraces = traces.Count,
            TotalValue = traces.Sum(t => t.Value),
            AverageValue = traces.Average(t => t.Value),
            LastTraceDate = traces.Max(t => t.DateSale),
            TotalTax = traces.Sum(t => t.Tax)
        };
    }
}