using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System;
using Api.Middleware;
using Infra;
using ListeDeNaissance.Core;
using ListeDeNaissance.API.Endpoints;
using Api.EndPoints;

var builder = WebApplication.CreateBuilder(args);

// 1. Enregistrement des services (TOUJOURS avant builder.Build())
builder.Services.AddOpenApi();
builder.Services.AddMonAppliCoreServices(); 
builder.Services.AddMyInfrastructureServices();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

// Configuration du CORS
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

builder.Services.AddAuthentication();

// 2. Construction de l'application (Une seule fois, après les services)
var app = builder.Build();

// 3. Configuration du pipeline HTTP
// Gestionnaire global des erreurs en tout premier
app.UseMiddleware<GlobalExceptionHandlerMiddleware>(); 

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Activation du CORS
app.UseCors("AllowLocalhost"); 

app.UseAuthentication();
app.UseAuthorization();

#region Endpoints
app.MapCompteRoutes();
app.AddArticleRoutes();
app.MapListeDeNaissanceRoutes();
app.MapModelDeListeRoutes();
#endregion

app.Run();