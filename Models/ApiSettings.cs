namespace MillionRealEstatecompany.API.Models;

public class ApiSettings
{
    public const string SectionName = "ApiSettings";
    
    public string Title { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ContactName { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
}