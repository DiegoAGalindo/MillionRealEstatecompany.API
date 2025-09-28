namespace MillionRealEstatecompany.API.Interfaces;

/// <summary>
/// Interfaz para el servicio de inserción de datos iniciales de prueba
/// </summary>
public interface IDataSeeder
{
    Task SeedDataAsync();
    Task<bool> HasDataAsync();
    Task ClearDataAsync();
}