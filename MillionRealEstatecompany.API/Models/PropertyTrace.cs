using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MillionRealEstatecompany.API.Models;

/// <summary>
/// Representa una traza o historial de transacciones de una propiedad (MongoDB)
/// </summary>
public class PropertyTrace
{
    /// <summary>
    /// Identificador único de MongoDB
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    /// <summary>
    /// Identificador numérico de la traza (compatibilidad API)
    /// </summary>
    [BsonElement("idPropertyTrace")]
    public int IdPropertyTrace { get; set; }

    /// <summary>
    /// Referencia al documento Property (ObjectId)
    /// </summary>
    [BsonElement("propertyId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string PropertyId { get; set; } = string.Empty;

    /// <summary>
    /// Identificador numérico de la propiedad (para queries más fáciles)
    /// </summary>
    [BsonElement("idProperty")]
    public int IdProperty { get; set; }

    /// <summary>
    /// Fecha de la transacción o evento
    /// </summary>
    [BsonElement("dateSale")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime DateSale { get; set; }

    /// <summary>
    /// Nombre o descripción del evento (compra, venta, avalúo, etc.)
    /// </summary>
    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Valor de la transacción en pesos colombianos
    /// </summary>
    [BsonElement("value")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Value { get; set; }

    /// <summary>
    /// Impuestos asociados a la transacción
    /// </summary>
    [BsonElement("tax")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Tax { get; set; }

    /// <summary>
    /// Fecha de creación del documento
    /// </summary>
    [BsonElement("createdAt")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}