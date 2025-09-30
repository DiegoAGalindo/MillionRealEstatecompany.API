using MillionRealEstatecompany.API.Models;

namespace MillionRealEstatecompany.API.Interfaces;

/// <summary>
/// Interface for property trace repository operations with MongoDB
/// </summary>
public interface IPropertyTraceRepository
{
    Task<IEnumerable<PropertyTrace>> GetAllAsync();
    Task<PropertyTrace?> GetByIdAsync(string id);
    Task<PropertyTrace?> GetByIdPropertyTraceAsync(int idPropertyTrace);
    Task<PropertyTrace> CreateAsync(PropertyTrace propertyTrace);
    Task<PropertyTrace?> UpdateAsync(string id, PropertyTrace propertyTrace);
    Task<bool> DeleteAsync(string id);
    Task<IEnumerable<PropertyTrace>> GetByPropertyIdAsync(string propertyId);
    Task<IEnumerable<PropertyTrace>> GetByIdPropertyAsync(int idProperty);
    Task<PropertyTrace?> GetLatestTraceByPropertyAsync(int idProperty);
    Task<int> GetNextIdPropertyTraceAsync();
    Task<PropertyTraceStatistics> GetStatisticsAsync(string propertyId);
}

/// <summary>
/// Estadísticas de traces de una propiedad
/// </summary>
public class PropertyTraceStatistics
{
    public int TotalTraces { get; set; }
    public decimal TotalValue { get; set; }
    public decimal AverageValue { get; set; }
    public DateTime LastTraceDate { get; set; }
    public decimal TotalTax { get; set; }
}