using System.ComponentModel.DataAnnotations;

namespace MillionRealEstatecompany.API.DTOs;

/// <summary>
/// DTO que representa un propietario en el sistema
/// </summary>
public class OwnerDto
{
    public int IdOwner { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? Photo { get; set; }
    public DateOnly Birthday { get; set; }
}

/// <summary>
/// DTO para crear un nuevo propietario
/// </summary>
public class CreateOwnerDto
{
    /// <summary>
    /// Nombre completo del propietario
    /// </summary>
    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Dirección del propietario
    /// </summary>
    [Required(ErrorMessage = "La dirección es obligatoria")]
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Foto del propietario (opcional)
    /// </summary>
    public string? Photo { get; set; }

    /// <summary>
    /// Fecha de nacimiento del propietario
    /// </summary>
    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
    public DateOnly? Birthday { get; set; }
}

public class UpdateOwnerDto
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? Photo { get; set; }
    public DateOnly Birthday { get; set; }
}