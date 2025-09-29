using System.ComponentModel.DataAnnotations;

namespace MillionRealEstatecompany.API.DTOs;

/// <summary>
/// DTO que representa un rastro de transacción de propiedad en el sistema
/// </summary>
public class PropertyTraceDto
{
    public int IdPropertyTrace { get; set; }
    public DateTime DateSale { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public decimal Tax { get; set; }
    public int IdProperty { get; set; }
}

/// <summary>
/// DTO para crear un nuevo rastro de transacción de propiedad
/// </summary>
public class CreatePropertyTraceDto
{
    /// <summary>
    /// Fecha de la transacción/venta
    /// </summary>
    [Required(ErrorMessage = "La fecha de venta es obligatoria")]
    public DateTime? DateSale { get; set; }

    /// <summary>
    /// Nombre o descripción de la transacción
    /// </summary>
    [Required(ErrorMessage = "El nombre de la transacción es obligatorio")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Valor de la transacción
    /// </summary>
    [Required(ErrorMessage = "El valor de la transacción es obligatorio")]
    public decimal? Value { get; set; }

    /// <summary>
    /// Impuesto aplicado a la transacción
    /// </summary>
    [Required(ErrorMessage = "El impuesto es obligatorio")]
    public decimal? Tax { get; set; }

    /// <summary>
    /// ID de la propiedad asociada a la transacción
    /// </summary>
    [Required(ErrorMessage = "El ID de la propiedad es obligatorio")]
    public int? IdProperty { get; set; }
}

public class UpdatePropertyTraceDto
{
    public DateTime DateSale { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public decimal Tax { get; set; }
    public int IdProperty { get; set; }
}