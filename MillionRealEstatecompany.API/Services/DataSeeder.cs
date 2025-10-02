using MillionRealEstatecompany.API.Data;
using MillionRealEstatecompany.API.Interfaces;
using MillionRealEstatecompany.API.Models;
using MongoDB.Driver;

namespace MillionRealEstatecompany.API.Services;

/// <summary>
/// Servicio responsable de insertar datos iniciales de prueba en MongoDB
/// Adaptado de la versión Entity Framework con los mismos datos de prueba
/// </summary>
public class DataSeeder : IDataSeeder
{
    private readonly MongoDbContext _context;
    private readonly ILogger<DataSeeder> _logger;

    public DataSeeder(MongoDbContext context, ILogger<DataSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Verifica si la base de datos ya contiene datos de propietarios
    /// </summary>
    /// <returns>True si existen propietarios en la base de datos</returns>
    public async Task<bool> HasDataAsync()
    {
        var count = await _context.Owners.CountDocumentsAsync(FilterDefinition<Owner>.Empty);
        return count > 0;
    }

    /// <summary>
    /// Elimina todos los datos existentes de la base de datos MongoDB
    /// </summary>
    public async Task ClearDataAsync()
    {
        await _context.PropertyTraces.DeleteManyAsync(FilterDefinition<PropertyTrace>.Empty);
        await _context.Properties.DeleteManyAsync(FilterDefinition<Property>.Empty);
        await _context.Owners.DeleteManyAsync(FilterDefinition<Owner>.Empty);
        
        _logger.LogInformation("All data cleared from MongoDB");
    }

    /// <summary>
    /// Inserta datos de prueba en MongoDB incluyendo propietarios, propiedades, imágenes y trazas
    /// Solo ejecuta si la base de datos está vacía
    /// </summary>
    /// <exception cref="Exception">Se propaga cualquier error ocurrido durante la inserción</exception>
    public async Task SeedDataAsync()
    {
        try
        {
            if (await HasDataAsync())
            {
                _logger.LogInformation("Data already exists in MongoDB. Skipping seeding.");
                return;
            }

            _logger.LogInformation("Starting MongoDB data seeding...");
            
            await SeedOwnersAsync();
            await SeedPropertiesAsync();
            await SeedPropertyTracesAsync();
            
            _logger.LogInformation("MongoDB data seeding completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during MongoDB data seeding.");
            throw;
        }
    }

    private async Task SeedOwnersAsync()
    {
        _logger.LogInformation("Seeding owners...");
        
        var owners = new List<Owner>
        {
            new() { 
                IdOwner = 1,
                Name = "Carlos Alberto Pérez", 
                Address = "Calle 123 #45-67, Bogotá", 
                Photo = "https://example.com/photos/carlos_perez.jpg", 
                Birthday = new DateTime(1975, 3, 15), 
                DocumentNumber = "12345678", 
                Email = "carlos.perez@example.com",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new() { 
                IdOwner = 2,
                Name = "María Elena Rodríguez", 
                Address = "Carrera 85 #120-34, Medellín", 
                Photo = "https://example.com/photos/maria_rodriguez.jpg", 
                Birthday = new DateTime(1982, 7, 22), 
                DocumentNumber = "23456789", 
                Email = "maria.rodriguez@example.com",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new() { 
                IdOwner = 3,
                Name = "Luis Fernando García", 
                Address = "Avenida 68 #25-89, Cali", 
                Photo = "https://example.com/photos/luis_garcia.jpg", 
                Birthday = new DateTime(1968, 11, 8), 
                DocumentNumber = "34567890", 
                Email = "luis.garcia@example.com",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new() { 
                IdOwner = 4,
                Name = "Ana Sofía Martínez", 
                Address = "Calle 72 #11-45, Barranquilla", 
                Photo = "https://example.com/photos/ana_martinez.jpg", 
                Birthday = new DateTime(1990, 2, 14), 
                DocumentNumber = "45678901", 
                Email = "ana.martinez@example.com",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new() { 
                IdOwner = 5,
                Name = "Roberto Carlos Silva", 
                Address = "Carrera 15 #93-12, Bucaramanga", 
                Photo = "https://example.com/photos/roberto_silva.jpg", 
                Birthday = new DateTime(1978, 9, 30), 
                DocumentNumber = "56789012", 
                Email = "roberto.silva@example.com",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new() { 
                IdOwner = 6,
                Name = "Carmen Lucía Herrera", 
                Address = "Calle 80 #45-23, Cartagena", 
                Photo = "https://example.com/photos/carmen_herrera.jpg", 
                Birthday = new DateTime(1985, 5, 18), 
                DocumentNumber = "67890123", 
                Email = "carmen.herrera@example.com",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new() { 
                IdOwner = 7,
                Name = "Diego Alejandro Morales", 
                Address = "Avenida 19 #104-56, Pereira", 
                Photo = "https://example.com/photos/diego_morales.jpg", 
                Birthday = new DateTime(1972, 12, 3), 
                DocumentNumber = "78901234", 
                Email = "diego.morales@example.com",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new() { 
                IdOwner = 8,
                Name = "Valentina González", 
                Address = "Carrera 50 #67-89, Manizales", 
                Photo = "https://example.com/photos/valentina_gonzalez.jpg", 
                Birthday = new DateTime(1987, 8, 25), 
                DocumentNumber = "89012345", 
                Email = "valentina.gonzalez@example.com",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new() { 
                IdOwner = 9,
                Name = "Andrés Felipe Torres", 
                Address = "Calle 26 #78-90, Armenia", 
                Photo = "https://example.com/photos/andres_torres.jpg", 
                Birthday = new DateTime(1980, 4, 11), 
                DocumentNumber = "90123456", 
                Email = "andres.torres@example.com",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new() { 
                IdOwner = 10,
                Name = "Isabella Castro", 
                Address = "Avenida 30 #12-34, Ibagué", 
                Photo = "https://example.com/photos/isabella_castro.jpg", 
                Birthday = new DateTime(1993, 1, 7), 
                DocumentNumber = "01234567", 
                Email = "isabella.castro@example.com",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new() { 
                IdOwner = 11,
                Name = "Sebastián Ramírez", 
                Address = "Carrera 40 #85-67, Neiva", 
                Photo = "https://example.com/photos/sebastian_ramirez.jpg", 
                Birthday = new DateTime(1974, 10, 19), 
                DocumentNumber = "10987654", 
                Email = "sebastian.ramirez@example.com",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new() { 
                IdOwner = 12,
                Name = "Camila Vargas", 
                Address = "Calle 60 #23-45, Villavicencio", 
                Photo = "https://example.com/photos/camila_vargas.jpg", 
                Birthday = new DateTime(1989, 6, 12), 
                DocumentNumber = "21098765", 
                Email = "camila.vargas@example.com",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new() { 
                IdOwner = 13,
                Name = "Fernando Ruiz", 
                Address = "Avenida 45 #67-12, Pasto", 
                Photo = "https://example.com/photos/fernando_ruiz.jpg", 
                Birthday = new DateTime(1971, 3, 28), 
                DocumentNumber = "32109876", 
                Email = "fernando.ruiz@example.com",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new() { 
                IdOwner = 14,
                Name = "Natalia López", 
                Address = "Carrera 25 #90-34, Popayán", 
                Photo = "https://example.com/photos/natalia_lopez.jpg", 
                Birthday = new DateTime(1986, 11, 15), 
                DocumentNumber = "43210987", 
                Email = "natalia.lopez@example.com",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new() { 
                IdOwner = 15,
                Name = "Gabriel Mendoza", 
                Address = "Calle 35 #56-78, Tunja", 
                Photo = "https://example.com/photos/gabriel_mendoza.jpg", 
                Birthday = new DateTime(1979, 7, 2), 
                DocumentNumber = "54321098", 
                Email = "gabriel.mendoza@example.com",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        await _context.Owners.InsertManyAsync(owners);
        _logger.LogInformation($"Seeded {owners.Count} owners");
    }

    private async Task SeedPropertiesAsync()
    {
        _logger.LogInformation("Seeding properties with embedded owners and images...");
        
        // Primero necesitamos obtener los propietarios ya creados
        var owners = await _context.Owners.Find(FilterDefinition<Owner>.Empty).ToListAsync();
        var ownerMap = owners.ToDictionary(o => o.IdOwner, o => o);
        
        var properties = new List<Property>();

        // Crear las 15 propiedades con sus propietarios embebidos
        var propertyData = new[]
        {
            new { Id = 1, Name = "Apartamento Zona Rosa", Address = "Calle 82 #11-23, Zona Rosa, Bogotá", Price = 450000000.00m, Code = "BOG-ZR-001", Year = 2019, OwnerId = 1 },
            new { Id = 2, Name = "Casa Campestre La Calera", Address = "Vereda El Salitre, La Calera", Price = 850000000.00m, Code = "CAL-CS-002", Year = 2020, OwnerId = 1 },
            new { Id = 3, Name = "Penthouse El Poblado", Address = "Carrera 43A #5-15, El Poblado, Medellín", Price = 1200000000.00m, Code = "MED-PH-003", Year = 2021, OwnerId = 2 },
            new { Id = 4, Name = "Apartamento Laureles", Address = "Calle 70 #80-45, Laureles, Medellín", Price = 380000000.00m, Code = "MED-LAU-004", Year = 2018, OwnerId = 2 },
            new { Id = 5, Name = "Oficina Centro Medellín", Address = "Carrera 50 #52-36, Centro, Medellín", Price = 250000000.00m, Code = "MED-OFC-005", Year = 2017, OwnerId = 2 },
            new { Id = 6, Name = "Casa San Antonio", Address = "Calle 1 Oeste #3-25, San Antonio, Cali", Price = 320000000.00m, Code = "CAL-SA-006", Year = 2016, OwnerId = 3 },
            new { Id = 7, Name = "Apartamento Ciudad Jardín", Address = "Avenida 6N #23-50, Ciudad Jardín, Cali", Price = 280000000.00m, Code = "CAL-CJ-007", Year = 2019, OwnerId = 3 },
            new { Id = 8, Name = "Casa El Prado", Address = "Calle 76 #68-45, El Prado, Barranquilla", Price = 420000000.00m, Code = "BAQ-EP-008", Year = 2020, OwnerId = 4 },
            new { Id = 9, Name = "Apartamento Norte Histórico", Address = "Carrera 46 #84-12, Norte Histórico, Barranquilla", Price = 195000000.00m, Code = "BAQ-NH-009", Year = 2015, OwnerId = 4 },
            new { Id = 10, Name = "Casa Cabecera", Address = "Carrera 33 #42-18, Cabecera, Bucaramanga", Price = 365000000.00m, Code = "BGA-CAB-010", Year = 2018, OwnerId = 5 },
            new { Id = 11, Name = "Casa Centro Histórico", Address = "Calle de la Factoria #36-57, Centro Histórico, Cartagena", Price = 680000000.00m, Code = "CTG-CH-012", Year = 2019, OwnerId = 6 },
            new { Id = 12, Name = "Casa Circunvalar", Address = "Avenida Circunvalar #15-78, Pereira", Price = 295000000.00m, Code = "PER-CIR-014", Year = 2017, OwnerId = 7 },
            new { Id = 13, Name = "Apartamento Zona Universitaria", Address = "Carrera 23 #65-12, Zona Universitaria, Manizales", Price = 215000000.00m, Code = "MZL-ZU-016", Year = 2018, OwnerId = 8 },
            new { Id = 14, Name = "Casa Centro Armenia", Address = "Carrera 14 #18-23, Centro, Armenia", Price = 225000000.00m, Code = "ARM-CEN-018", Year = 2015, OwnerId = 9 },
            new { Id = 15, Name = "Casa Ambalá", Address = "Carrera 5 #60-34, Ambalá, Ibagué", Price = 190000000.00m, Code = "IBA-AMB-020", Year = 2018, OwnerId = 10 }
        };

        foreach (var prop in propertyData)
        {
            if (ownerMap.TryGetValue(prop.OwnerId, out var owner))
            {
                properties.Add(new Property
                {
                    IdProperty = prop.Id,
                    Name = prop.Name,
                    Address = prop.Address,
                    Price = prop.Price,
                    CodeInternal = prop.Code,
                    Year = prop.Year,
                    Owner = new EmbeddedOwner 
                    { 
                        IdOwner = owner.IdOwner, 
                        Name = owner.Name, 
                        Address = owner.Address, 
                        Photo = owner.Photo ?? "" 
                    },
                    Images = new List<EmbeddedPropertyImage>
                    {
                        new() { IdPropertyImage = prop.Id * 3 - 2, File = $"https://million-storage.com/properties/prop-{prop.Id:D3}/facade.jpg", Enabled = true },
                        new() { IdPropertyImage = prop.Id * 3 - 1, File = $"https://million-storage.com/properties/prop-{prop.Id:D3}/interior.jpg", Enabled = true },
                        new() { IdPropertyImage = prop.Id * 3, File = $"https://million-storage.com/properties/prop-{prop.Id:D3}/view.jpg", Enabled = true }
                    },
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }
        }

        await _context.Properties.InsertManyAsync(properties);
        _logger.LogInformation($"Seeded {properties.Count} properties with embedded owners and images");
    }

    private async Task SeedPropertyTracesAsync()
    {
        _logger.LogInformation("Seeding property traces...");
        
        // Primero obtenemos todas las propiedades para obtener sus ObjectIds
        var properties = await _context.Properties.Find(FilterDefinition<Property>.Empty).ToListAsync();
        var propertyMap = properties.ToDictionary(p => p.IdProperty, p => p.Id);
        
        var traces = new List<PropertyTrace>();
        var random = new Random();
        int traceIdCounter = 1;

        foreach (var property in properties)
        {
            var baseValue = random.Next(100000000, 800000000);
            var purchaseDate = DateTime.UtcNow.AddYears(-random.Next(1, 6));

            traces.AddRange(new[]
            {
                new PropertyTrace
                {
                    IdPropertyTrace = traceIdCounter++,
                    PropertyId = property.Id ?? string.Empty,
                    IdProperty = property.IdProperty,
                    DateSale = purchaseDate,
                    Name = "Compra inicial",
                    Value = baseValue,
                    Tax = baseValue * 0.03m
                },
                new PropertyTrace
                {
                    IdPropertyTrace = traceIdCounter++,
                    PropertyId = property.Id ?? string.Empty,
                    IdProperty = property.IdProperty,
                    DateSale = purchaseDate.AddMonths(random.Next(6, 24)),
                    Name = "Avalúo comercial",
                    Value = baseValue * 1.1m,
                    Tax = 0
                }
            });
        }

        await _context.PropertyTraces.InsertManyAsync(traces);
        _logger.LogInformation($"Seeded {traces.Count} property traces");
    }
}