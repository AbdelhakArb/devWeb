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

// -----------------------------------------------------------------
// 1. CONFIGURATION DES SERVICES (AVANT builder.Build())
// -----------------------------------------------------------------

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configuration de la politique CORS pour autoriser Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Ton port Angular
              .AllowAnyHeader()                     // Autorise le Content-Type, les tokens, etc.
              .AllowAnyMethod()                     // Autorise POST, GET, PUT, DELETE
              .AllowCredentials();                  // Optionnel : Autorise les cookies/sessions si besoin
    });
});

// Exemple de configuration de ta base de données (à adapter avec ton vrai DbContext)
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// -----------------------------------------------------------------
// 2. CONFIGURATION DU PIPELINE HTTP (APRÈS builder.Build())
// -----------------------------------------------------------------


// /!\ L'ORDRE ICI EST CRUCIAL POUR EVITER LES ERREURS /!\

app.UseRouting();

// Activation globale du CORS (Placé obligatoirement AVANT l'authentification/autorisation)
app.UseCors("AllowAngularApp");

app.UseHttpsRedirection(); // Si tu utilises aussi du HTTPS en parallèle

app.UseAuthorization();

app.MapControllers();

app.Run();