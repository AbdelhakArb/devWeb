using Api.Middleware;
using Infra;                        // Ton bon using pour l'infrastructure
using ListeDeNaissance.Core;        // Ton bon using pour le Core
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using ListeDeNaissance.API.Endpoints;
using Api.EndPoints;

var builder = WebApplication.CreateBuilder(args);

// Enregistrement des services de ton application
builder.Services.AddOpenApi();

// =========================================================================
// 🔌 CONNEXION DE TES SERVICES (100 % CORRIGÉ AVEC TES VRAIS NOMS)
// =========================================================================
builder.Services.AddMonAppliCoreServices(); 
builder.Services.AddMyInfrastructureServices();

#region Authentication and Authorization


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

#endregion

#region Configuration du CORS (Pour autoriser ton Angular)

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
builder.Services.AddAuthentication();
var app = builder.Build();

// Gestionnaire global des erreurs
app.UseMiddleware<GlobalExceptionHandlerMiddleware>(); 

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseCors("AllowLocalhost"); 
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


#region Endpoints (Routes de ton application Liste de Naissance)

// On active uniquement TES routes d'authentification
app.MapCompteRoutes(); 

#endregion

app.Run();