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
    public DateTime Birthday { get; set; }
    public string DocumentNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
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
    public DateTime Birthday { get; set; }

    /// <summary>
    /// Número de documento de identificación del propietario
    /// </summary>
    [Required(ErrorMessage = "El número de documento es obligatorio")]
    [StringLength(20, MinimumLength = 5, ErrorMessage = "El número de documento debe tener entre 5 y 20 caracteres")]
    public string DocumentNumber { get; set; } = string.Empty;

    /// <summary>
    /// Correo electrónico del propietario
    /// </summary>
    [Required(ErrorMessage = "El correo electrónico es obligatorio")]
    [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
    [StringLength(100, ErrorMessage = "El correo electrónico no puede exceder 100 caracteres")]
    public string Email { get; set; } = string.Empty;
}

/// <summary>
/// DTO para actualizar un propietario existente
/// </summary>
public class UpdateOwnerDto
{
    /// <summary>
    /// Nombre completo del propietario (opcional)
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Dirección del propietario (opcional)
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Foto del propietario (opcional)
    /// </summary>
    public string? Photo { get; set; }

    /// <summary>
    /// Fecha de nacimiento (opcional)
    /// </summary>
    public DateTime? Birthday { get; set; }

    /// <summary>
    /// Correo electrónico del propietario (opcional)
    /// </summary>
    [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
    [StringLength(100, ErrorMessage = "El correo electrónico no puede exceder 100 caracteres")]
    public string? Email { get; set; }
}
