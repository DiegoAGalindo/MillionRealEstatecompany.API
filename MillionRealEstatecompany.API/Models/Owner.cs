using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MillionRealEstatecompany.API.Models;

/// <summary>
/// Representa un propietario de inmuebles en el sistema MongoDB
/// </summary>
public class Owner
{
    /// <summary>
    /// Identificador único de MongoDB
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    /// <summary>
    /// Identificador numérico del propietario (compatibilidad API)
    /// </summary>
    [BsonElement("idOwner")]
    public int IdOwner { get; set; }

    /// <summary>
    /// Nombre completo del propietario
    /// </summary>
    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Dirección de residencia del propietario
    /// </summary>
    [BsonElement("address")]
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// URL de la foto del propietario (opcional)
    /// </summary>
    [BsonElement("photo")]
    public string? Photo { get; set; }

    /// <summary>
    /// Fecha de nacimiento del propietario (solo fecha, sin hora)
    /// </summary>
    [BsonElement("birthday")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime Birthday { get; set; }

    /// <summary>
    /// Número de documento de identificación del propietario (único)
    /// </summary>
    [BsonElement("documentNumber")]
    public string DocumentNumber { get; set; } = string.Empty;

    /// <summary>
    /// Correo electrónico del propietario
    /// </summary>
    [BsonElement("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Número de propiedades que posee (metadata)
    /// </summary>
    [BsonElement("propertiesCount")]
    public int PropertiesCount { get; set; } = 0;

    /// <summary>
    /// Fecha de creación del documento
    /// </summary>
    [BsonElement("createdAt")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Fecha de última actualización
    /// </summary>
    [BsonElement("updatedAt")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
