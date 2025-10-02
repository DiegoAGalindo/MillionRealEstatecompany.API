using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using MillionRealEstatecompany.API.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MillionRealEstatecompany.API.Data;

/// <summary>
/// Contexto de MongoDB para Million Real Estate
/// </summary>
public class MongoDbContext
{
    private readonly IMongoDatabase _database;
    private readonly ILogger<MongoDbContext>? _logger;

    public MongoDbContext(IOptions<MongoDbSettings> settings, ILogger<MongoDbContext>? logger = null)
    {
        _logger = logger;
        
        try
        {
            _logger?.LogInformation("Connecting to MongoDB with connection string: {ConnectionString}", 
                settings.Value.ConnectionString?.Replace(":password123@", ":***@"));
            
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
            
            _logger?.LogInformation("Connected to MongoDB database: {DatabaseName}", settings.Value.DatabaseName);
            
            // Configurar índices al inicializar
            ConfigureIndexes();
            
            _logger?.LogInformation("MongoDB indexes configured successfully");
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to connect to MongoDB");
            throw;
        }
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
    /// Verifica la conexión a MongoDB
    /// </summary>
    public async Task<bool> TestConnectionAsync()
    {
        try
        {
            await _database.RunCommandAsync((Command<BsonDocument>)"{ping:1}");
            _logger?.LogInformation("MongoDB connection test successful");
            return true;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "MongoDB connection test failed");
            return false;
        }
    }

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

/// <summary>
/// Health check para MongoDB
/// </summary>
public class MongoDbHealthCheck : IHealthCheck
{
    private readonly MongoDbContext _context;

    public MongoDbHealthCheck(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var isHealthy = await _context.TestConnectionAsync();
            return isHealthy 
                ? HealthCheckResult.Healthy("MongoDB is responding normally")
                : HealthCheckResult.Unhealthy("MongoDB is not responding");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("MongoDB health check failed", ex);
        }
    }
}