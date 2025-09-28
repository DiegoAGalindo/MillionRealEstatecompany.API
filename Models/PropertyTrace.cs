using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MillionRealEstatecompany.API.Models;

/// <summary>
/// Representa una traza o historial de transacciones de una propiedad
/// </summary>
public class PropertyTrace
{
    /// <summary>
    /// Identificador único de la traza
    /// </summary>
    [Key]
    public int IdPropertyTrace { get; set; }

    /// <summary>
    /// Fecha de la transacción o evento
    /// </summary>
    public DateTime DateSale { get; set; }

    /// <summary>
    /// Nombre o descripción del evento (compra, venta, avalúo, etc.)
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Valor de la transacción en pesos colombianos
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal Value { get; set; }

    /// <summary>
    /// Impuestos asociados a la transacción
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal Tax { get; set; }

    /// <summary>
    /// Identificador de la propiedad asociada
    /// </summary>
    [ForeignKey("Property")]
    public int IdProperty { get; set; }

    /// <summary>
    /// Propiedad asociada a esta traza
    /// </summary>
    public virtual Property Property { get; set; } = null!;
}