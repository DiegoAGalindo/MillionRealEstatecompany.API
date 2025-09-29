using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace MillionRealEstatecompany.API.Middleware;

/// <summary>
/// Middleware para validación automática de modelos
/// </summary>
public class ModelValidationMiddleware
{
    private readonly RequestDelegate _next;

    public ModelValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        // Interceptar respuestas de validación de modelo
        if (context.Response.StatusCode == (int)HttpStatusCode.BadRequest)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<ApiControllerAttribute>() != null)
            {
                // El endpoint ya maneja la validación del modelo automáticamente
                // No necesitamos hacer nada adicional aquí
            }
        }
    }
}