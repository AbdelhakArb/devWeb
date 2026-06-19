using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Middleware;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // On laisse la requête continuer son chemin vers les Endpoints / UseCases
            await _next(context);
        }
        catch (Exception ex)
        {
            // Si un bug non géré survient n'importe où, on l'attrape ici !
            _logger.LogError(ex, "Une erreur non gérée est survenue : {Message}", ex.Message);
            
            // On configure une réponse propre pour le client (HTML / React / Vue)
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        // Par défaut, on renvoie un code 500 (Erreur Interne du Serveur)
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var reponseErreur = new
        {
            StatusCode = context.Response.StatusCode,
            Message = "Une erreur interne est survenue sur le serveur. Veuillez réessayer plus tard.",
            Detailed = exception.Message // Optionnel : pratique pour debugger pendant le dev
        };

        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        return context.Response.WriteAsync(JsonSerializer.Serialize(reponseErreur, jsonOptions));
    }
}