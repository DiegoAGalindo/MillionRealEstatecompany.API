using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MillionRealEstatecompany.API.Models;

/// <summary>
/// Representa una propiedad inmobiliaria en el sistema MongoDB
/// </summary>
public class Property
{
    /// <summary>
    /// Identificador único de MongoDB
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    /// <summary>
    /// Identificador numérico de la propiedad (compatibilidad API)
    /// </summary>
    [BsonElement("idProperty")]
    public int IdProperty { get; set; }

    /// <summary>
    /// Nombre o título de la propiedad
    /// </summary>
    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Dirección física de la propiedad
    /// </summary>
    [BsonElement("address")]
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Precio de la propiedad en pesos colombianos
    /// </summary>
    [BsonElement("price")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Price { get; set; }

    /// <summary>
    /// Código interno único de la propiedad
    /// </summary>
    [BsonElement("codeInternal")]
    public string CodeInternal { get; set; } = string.Empty;

    /// <summary>
    /// Año de construcción de la propiedad
    /// </summary>
    [BsonElement("year")]
    public int Year { get; set; }

    /// <summary>
    /// Propietario embebido en el documento
    /// </summary>
    [BsonElement("owner")]
    public EmbeddedOwner Owner { get; set; } = new();

    /// <summary>
    /// Imágenes embebidas en el documento
    /// </summary>
    [BsonElement("images")]
    public List<EmbeddedPropertyImage> Images { get; set; } = new();

    /// <summary>
    /// Indica si la propiedad está habilitada
    /// </summary>
    [BsonElement("enabled")]
    public bool Enabled { get; set; } = true;

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

/// <summary>
/// Propietario embebido en el documento Property
/// </summary>
public class EmbeddedOwner
{
    [BsonElement("idOwner")]
    public int IdOwner { get; set; }

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("address")]
    public string Address { get; set; } = string.Empty;

    [BsonElement("photo")]
    public string Photo { get; set; } = string.Empty;
}

/// <summary>
/// Imagen embebida en el documento Property
/// </summary>
public class EmbeddedPropertyImage
{
    [BsonElement("idPropertyImage")]
    public int IdPropertyImage { get; set; }

    [BsonElement("file")]
    public string File { get; set; } = string.Empty;

    [BsonElement("enabled")]
    public bool Enabled { get; set; } = true;
}