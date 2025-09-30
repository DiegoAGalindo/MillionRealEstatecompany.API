using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MillionRealEstatecompany.API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MillionRealEstatecompany.API.Controllers;

/// <summary>
/// Controlador simple para autenticación JWT
/// Solo para validar autorización en controladores
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IOptions<JwtSettings> jwtSettings, ILogger<AuthController> logger)
    {
        _jwtSettings = jwtSettings.Value;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene un token JWT con credenciales fijas
    /// </summary>
    /// <param name="request">Credenciales de login</param>
    /// <returns>Token JWT</returns>
    /// <response code="200">Login exitoso, devuelve token JWT</response>
    /// <response code="401">Credenciales incorrectas</response>
    [HttpPost("login")]
    [AllowAnonymous]
    public ActionResult<object> Login([FromBody] LoginRequest request)
    {
        // Validar credenciales fijas
        if (request.Username == "testmillion" && request.Password == "TestMillionPass")
        {
            var token = GenerateJwtToken();
            return Ok(new { token, message = "Login exitoso" });
        }

        return Unauthorized(new { message = "Credenciales incorrectas" });
    }

    private string GenerateJwtToken()
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "testmillion"),
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
            signingCredentials: credentials
        );
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return $"Bearer {tokenString}";
    }
}

/// <summary>
/// Modelo simple para el login
/// </summary>
/// <example>
/// {
///   "username": "testmillion",
///   "password": "TestMillionPass"
/// }
/// </example>
public class LoginRequest
{
    /// <summary>
    /// Nombre de usuario
    /// </summary>
    /// <example>testmillion</example>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Contraseña
    /// </summary>
    /// <example>TestMillionPass</example>
    public string Password { get; set; } = string.Empty;
}