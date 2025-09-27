namespace MillionRealEstatecompany.API.Interfaces;

public interface IDataSeeder
{
    Task SeedDataAsync();
    Task<bool> HasDataAsync();
    Task ClearDataAsync();
}