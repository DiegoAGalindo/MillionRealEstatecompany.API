namespace MillionRealEstatecompany.API.Models;

public class DatabaseSettings
{
    public const string SectionName = "DatabaseSettings";
    
    public int CommandTimeout { get; set; } = 30;
    public bool EnableSensitiveDataLogging { get; set; } = false;
    public bool EnableDetailedErrors { get; set; } = false;
}