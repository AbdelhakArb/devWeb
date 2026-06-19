using Api.Middleware;
using Api.EndPoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

// Importation de tes propres extensions de services et endpoints
using Infra; 
using ListeDeNaissance.Core;
using ListeDeNaissance.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// =========================================================================
// CONFIGURATION DES SERVICES
// =========================================================================

// Enregistrement de TES services (Injections de ton application)
builder.Services.AddMyInfrastructureServices();
builder.Services.AddMonAppliCoreServices();

#region Cors

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy =>
        {
            policy.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

#endregion

var app = builder.Build();

// Notre filet de sécurité global pour attraper les crashs proprement
app.UseMiddleware<GlobalExceptionHandlerMiddleware>(); 

if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowLocalhost");
}

app.UseHttpsRedirection();

#region Endpoints (TES ROUTES)

app.MapCompteRoutes();          
app.MapListeDeNaissanceRoutes();    

#endregion

app.Run();