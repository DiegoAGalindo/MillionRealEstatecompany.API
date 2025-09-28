using MillionRealEstatecompany.API.Data;
using MillionRealEstatecompany.API.Interfaces;
using MillionRealEstatecompany.API.Models;
using Microsoft.EntityFrameworkCore;

namespace MillionRealEstatecompany.API.Services;

/// <summary>
/// Servicio responsable de insertar datos iniciales de prueba en la base de datos
/// </summary>
public class DataSeeder : IDataSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DataSeeder> _logger;

    public DataSeeder(ApplicationDbContext context, ILogger<DataSeeder> logger)
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
        return await _context.Owners.AnyAsync();
    }

    /// <summary>
    /// Elimina todos los datos existentes de la base de datos en el orden correcto para evitar conflictos de claves foráneas
    /// </summary>
    public async Task ClearDataAsync()
    {
        _context.PropertyTraces.RemoveRange(_context.PropertyTraces);
        _context.PropertyImages.RemoveRange(_context.PropertyImages);
        _context.Properties.RemoveRange(_context.Properties);
        _context.Owners.RemoveRange(_context.Owners);

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Inserta datos de prueba en la base de datos incluyendo propietarios, propiedades, imágenes y trazas
    /// Solo ejecuta si la base de datos está vacía
    /// </summary>
    /// <exception cref="Exception">Se propaga cualquier error ocurrido durante la inserción</exception>
    public async Task SeedDataAsync()
    {
        try
        {
            if (await HasDataAsync())
            {
                return;
            }

            await SeedOwnersAsync();
            await SeedPropertiesAsync();
            await SeedPropertyImagesAsync();
            await SeedPropertyTracesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during data seeding.");
            throw;
        }
    }

    private async Task SeedOwnersAsync()
    {
        var owners = new List<Owner>
        {
            new() { Name = "Carlos Alberto Pérez", Address = "Calle 123 #45-67, Bogotá", Photo = "https://example.com/photos/carlos_perez.jpg", Birthday = new DateOnly(1975, 3, 15) },
            new() { Name = "María Elena Rodríguez", Address = "Carrera 85 #120-34, Medellín", Photo = "https://example.com/photos/maria_rodriguez.jpg", Birthday = new DateOnly(1982, 7, 22) },
            new() { Name = "Luis Fernando García", Address = "Avenida 68 #25-89, Cali", Photo = "https://example.com/photos/luis_garcia.jpg", Birthday = new DateOnly(1968, 11, 8) },
            new() { Name = "Ana Sofía Martínez", Address = "Calle 72 #11-45, Barranquilla", Photo = "https://example.com/photos/ana_martinez.jpg", Birthday = new DateOnly(1990, 2, 14) },
            new() { Name = "Roberto Carlos Silva", Address = "Carrera 15 #93-12, Bucaramanga", Photo = "https://example.com/photos/roberto_silva.jpg", Birthday = new DateOnly(1978, 9, 30) },
            new() { Name = "Carmen Lucía Herrera", Address = "Calle 80 #45-23, Cartagena", Photo = "https://example.com/photos/carmen_herrera.jpg", Birthday = new DateOnly(1985, 5, 18) },
            new() { Name = "Diego Alejandro Morales", Address = "Avenida 19 #104-56, Pereira", Photo = "https://example.com/photos/diego_morales.jpg", Birthday = new DateOnly(1972, 12, 3) },
            new() { Name = "Valentina González", Address = "Carrera 50 #67-89, Manizales", Photo = "https://example.com/photos/valentina_gonzalez.jpg", Birthday = new DateOnly(1987, 8, 25) },
            new() { Name = "Andrés Felipe Torres", Address = "Calle 26 #78-90, Armenia", Photo = "https://example.com/photos/andres_torres.jpg", Birthday = new DateOnly(1980, 4, 11) },
            new() { Name = "Isabella Castro", Address = "Avenida 30 #12-34, Ibagué", Photo = "https://example.com/photos/isabella_castro.jpg", Birthday = new DateOnly(1993, 1, 7) },
            new() { Name = "Sebastián Ramírez", Address = "Carrera 40 #85-67, Neiva", Photo = "https://example.com/photos/sebastian_ramirez.jpg", Birthday = new DateOnly(1974, 10, 19) },
            new() { Name = "Camila Vargas", Address = "Calle 60 #23-45, Villavicencio", Photo = "https://example.com/photos/camila_vargas.jpg", Birthday = new DateOnly(1989, 6, 12) },
            new() { Name = "Fernando Ruiz", Address = "Avenida 45 #67-12, Pasto", Photo = "https://example.com/photos/fernando_ruiz.jpg", Birthday = new DateOnly(1971, 3, 28) },
            new() { Name = "Natalia López", Address = "Carrera 25 #90-34, Popayán", Photo = "https://example.com/photos/natalia_lopez.jpg", Birthday = new DateOnly(1986, 11, 15) },
            new() { Name = "Gabriel Mendoza", Address = "Calle 35 #56-78, Tunja", Photo = "https://example.com/photos/gabriel_mendoza.jpg", Birthday = new DateOnly(1979, 7, 2) }
        };

        _context.Owners.AddRange(owners);
        await _context.SaveChangesAsync();
    }

    private async Task SeedPropertiesAsync()
    {
        var properties = new List<Property>
        {
            new() { Name = "Apartamento Zona Rosa", Address = "Calle 82 #11-23, Zona Rosa, Bogotá", Price = 450000000.00m, CodeInternal = "BOG-ZR-001", Year = 2019, IdOwner = 1 },
            new() { Name = "Casa Campestre La Calera", Address = "Vereda El Salitre, La Calera", Price = 850000000.00m, CodeInternal = "CAL-CS-002", Year = 2020, IdOwner = 1 },
            new() { Name = "Penthouse El Poblado", Address = "Carrera 43A #5-15, El Poblado, Medellín", Price = 1200000000.00m, CodeInternal = "MED-PH-003", Year = 2021, IdOwner = 2 },
            new() { Name = "Apartamento Laureles", Address = "Calle 70 #80-45, Laureles, Medellín", Price = 380000000.00m, CodeInternal = "MED-LAU-004", Year = 2018, IdOwner = 2 },
            new() { Name = "Oficina Centro Medellín", Address = "Carrera 50 #52-36, Centro, Medellín", Price = 250000000.00m, CodeInternal = "MED-OFC-005", Year = 2017, IdOwner = 2 },
            new() { Name = "Casa San Antonio", Address = "Calle 1 Oeste #3-25, San Antonio, Cali", Price = 320000000.00m, CodeInternal = "CAL-SA-006", Year = 2016, IdOwner = 3 },
            new() { Name = "Apartamento Ciudad Jardín", Address = "Avenida 6N #23-50, Ciudad Jardín, Cali", Price = 280000000.00m, CodeInternal = "CAL-CJ-007", Year = 2019, IdOwner = 3 },
            new() { Name = "Casa El Prado", Address = "Calle 76 #68-45, El Prado, Barranquilla", Price = 420000000.00m, CodeInternal = "BAQ-EP-008", Year = 2020, IdOwner = 4 },
            new() { Name = "Apartamento Norte Histórico", Address = "Carrera 46 #84-12, Norte Histórico, Barranquilla", Price = 195000000.00m, CodeInternal = "BAQ-NH-009", Year = 2015, IdOwner = 4 },
            new() { Name = "Casa Cabecera", Address = "Carrera 33 #42-18, Cabecera, Bucaramanga", Price = 365000000.00m, CodeInternal = "BGA-CAB-010", Year = 2018, IdOwner = 5 },
            new() { Name = "Casa Centro Histórico", Address = "Calle de la Factoria #36-57, Centro Histórico, Cartagena", Price = 680000000.00m, CodeInternal = "CTG-CH-012", Year = 2019, IdOwner = 6 },
            new() { Name = "Casa Circunvalar", Address = "Avenida Circunvalar #15-78, Pereira", Price = 295000000.00m, CodeInternal = "PER-CIR-014", Year = 2017, IdOwner = 7 },
            new() { Name = "Apartamento Zona Universitaria", Address = "Carrera 23 #65-12, Zona Universitaria, Manizales", Price = 215000000.00m, CodeInternal = "MZL-ZU-016", Year = 2018, IdOwner = 8 },
            new() { Name = "Casa Centro Armenia", Address = "Carrera 14 #18-23, Centro, Armenia", Price = 225000000.00m, CodeInternal = "ARM-CEN-018", Year = 2015, IdOwner = 9 },
            new() { Name = "Casa Ambalá", Address = "Carrera 5 #60-34, Ambalá, Ibagué", Price = 190000000.00m, CodeInternal = "IBA-AMB-020", Year = 2018, IdOwner = 10 }
        };

        _context.Properties.AddRange(properties);
        await _context.SaveChangesAsync();
    }

    private async Task SeedPropertyImagesAsync()
    {
        var images = new List<PropertyImage>();

        for (int propertyId = 1; propertyId <= 15; propertyId++)
        {
            images.AddRange(new[]
            {
                new PropertyImage { IdProperty = propertyId, File = $"https://million-storage.com/properties/prop-{propertyId:D3}/facade.jpg", Enabled = true },
                new PropertyImage { IdProperty = propertyId, File = $"https://million-storage.com/properties/prop-{propertyId:D3}/interior.jpg", Enabled = true },
                new PropertyImage { IdProperty = propertyId, File = $"https://million-storage.com/properties/prop-{propertyId:D3}/view.jpg", Enabled = true }
            });
        }

        _context.PropertyImages.AddRange(images);
        await _context.SaveChangesAsync();
    }

    private async Task SeedPropertyTracesAsync()
    {
        var traces = new List<PropertyTrace>();
        var random = new Random();

        for (int propertyId = 1; propertyId <= 15; propertyId++)
        {
            var baseValue = random.Next(100000000, 800000000);
            var purchaseDate = DateTime.UtcNow.AddYears(-random.Next(1, 6));

            traces.AddRange(new[]
            {
                new PropertyTrace
                {
                    IdProperty = propertyId,
                    DateSale = purchaseDate,
                    Name = "Compra inicial",
                    Value = baseValue,
                    Tax = baseValue * 0.03m
                },
                new PropertyTrace
                {
                    IdProperty = propertyId,
                    DateSale = purchaseDate.AddMonths(random.Next(6, 24)),
                    Name = "Avalúo comercial",
                    Value = baseValue * 1.1m,
                    Tax = 0
                }
            });
        }

        _context.PropertyTraces.AddRange(traces);
        await _context.SaveChangesAsync();
    }
}