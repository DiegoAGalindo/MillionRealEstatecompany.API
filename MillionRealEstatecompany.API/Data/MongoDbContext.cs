using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MillionRealEstatecompany.API.Models;

namespace MillionRealEstatecompany.API.Data;

/// <summary>
/// Contexto de MongoDB para Million Real Estate
/// </summary>
public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
        
        // Configurar índices al inicializar
        ConfigureIndexes();
    }

    /// <summary>
    /// Colección de Propiedades
    /// </summary>
    public IMongoCollection<Property> Properties => _database.GetCollection<Property>("properties");

    /// <summary>
    /// Colección de Propietarios
    /// </summary>
    public IMongoCollection<Owner> Owners => _database.GetCollection<Owner>("owners");

    /// <summary>
    /// Colección de Trazas de Propiedades
    /// </summary>
    public IMongoCollection<PropertyTrace> PropertyTraces => _database.GetCollection<PropertyTrace>("propertytraces");

    /// <summary>
    /// Configura índices para optimizar las consultas
    /// </summary>
    private void ConfigureIndexes()
    {
        // Índices para Properties
        var propertiesIndexes = Builders<Property>.IndexKeys;
        Properties.Indexes.CreateOneAsync(new CreateIndexModel<Property>(
            propertiesIndexes.Ascending(p => p.IdProperty),
            new CreateIndexOptions { Unique = true, Name = "idx_property_id" }
        ));
        Properties.Indexes.CreateOneAsync(new CreateIndexModel<Property>(
            propertiesIndexes.Ascending(p => p.CodeInternal),
            new CreateIndexOptions { Unique = true, Name = "idx_property_code" }
        ));
        Properties.Indexes.CreateOneAsync(new CreateIndexModel<Property>(
            propertiesIndexes.Ascending("owner.idOwner"),
            new CreateIndexOptions { Name = "idx_property_owner" }
        ));

        // Índices para Owners
        var ownersIndexes = Builders<Owner>.IndexKeys;
        Owners.Indexes.CreateOneAsync(new CreateIndexModel<Owner>(
            ownersIndexes.Ascending(o => o.IdOwner),
            new CreateIndexOptions { Unique = true, Name = "idx_owner_id" }
        ));
        Owners.Indexes.CreateOneAsync(new CreateIndexModel<Owner>(
            ownersIndexes.Ascending(o => o.DocumentNumber),
            new CreateIndexOptions { Unique = true, Name = "idx_owner_document" }
        ));
        Owners.Indexes.CreateOneAsync(new CreateIndexModel<Owner>(
            ownersIndexes.Ascending(o => o.Email),
            new CreateIndexOptions { Unique = true, Name = "idx_owner_email" }
        ));

        // Índices para PropertyTraces
        var tracesIndexes = Builders<PropertyTrace>.IndexKeys;
        PropertyTraces.Indexes.CreateOneAsync(new CreateIndexModel<PropertyTrace>(
            tracesIndexes.Ascending(pt => pt.IdPropertyTrace),
            new CreateIndexOptions { Unique = true, Name = "idx_trace_id" }
        ));
        PropertyTraces.Indexes.CreateOneAsync(new CreateIndexModel<PropertyTrace>(
            tracesIndexes.Ascending(pt => pt.PropertyId),
            new CreateIndexOptions { Name = "idx_trace_property" }
        ));
        PropertyTraces.Indexes.CreateOneAsync(new CreateIndexModel<PropertyTrace>(
            tracesIndexes.Ascending(pt => pt.IdProperty),
            new CreateIndexOptions { Name = "idx_trace_property_num" }
        ));
        PropertyTraces.Indexes.CreateOneAsync(new CreateIndexModel<PropertyTrace>(
            tracesIndexes.Descending(pt => pt.DateSale),
            new CreateIndexOptions { Name = "idx_trace_date" }
        ));
    }
}