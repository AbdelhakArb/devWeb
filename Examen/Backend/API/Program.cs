using ListeDeNaissance.Core.IGateways;
using ListeDeNaissance.Core.Usecases;
using ListeDeNaissance.Core.Usecases.Abstractions;
using Infra.Repositories;
using Infra.Repositories.Abstractions;
using Api.EndPoints; // <-- On ajoute ça pour lier tes nouvelles routes !

var builder = WebApplication.CreateBuilder(args);

// =========================================================================
// 1. INJECTION DES DÉPENDANCES
// =========================================================================

// ---- Les Gateways / Repositories ----
builder.Services.AddScoped<IListeDeNaissanceGateway, ListeDeNaissanceRepository>();
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<ICompteRepository, CompteRepository>();

// ---- Les UseCases ----
builder.Services.AddScoped<ICreerListeDeNaissanceUseCase, CreerListeDeNaissanceUseCase>();

// Configuration OpenAPI (Swagger)
builder.Services.AddOpenApi();

var app = builder.Build();

// =========================================================================
// 2. CONFIGURATION DU PIPELINE HTTP
// =========================================================================

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Enregistrement de tes routes
app.MapListeDeNaissanceRoutes();

app.Run();