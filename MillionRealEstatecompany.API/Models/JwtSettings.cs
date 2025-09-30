namespace MillionRealEstatecompany.API.Models;

/// <summary>
/// Configuración para MongoDB
/// </summary>
public class MongoDbSettings
{
    public const string SectionName = "MongoDbSettings";

    /// <summary>
    /// Cadena de conexión a MongoDB
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Nombre de la base de datos
    /// </summary>
    public string DatabaseName { get; set; } = string.Empty;
}

/// <summary>
/// Configuración de JWT
/// </summary>
public class JwtSettings
{
    public const string SectionName = "JwtSettings";

    public string SecretKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpirationInMinutes { get; set; } = 60;
}