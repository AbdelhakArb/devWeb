using ListeDeNaissance.Core.IGateways;
using ListeDeNaissance.Core.Usecases;
using ListeDeNaissance.Core.Usecases.Abstractions;
using Infra.Repositories;
using Infra.Repositories.Abstractions;
using Api.EndPoints; 

var builder = WebApplication.CreateBuilder(args);

// =========================================================================
// 1. CONFIGURATION DES CORS (Pour autoriser Angular)
// =========================================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy.WithOrigins("http://localhost:4200") // L'URL par défaut d'Angular
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

// =========================================================================
// 2. INJECTION DES DÉPENDANCES
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
// 3. CONFIGURATION DU PIPELINE HTTP
// =========================================================================

// Activation obligatoire des CORS dans le pipeline (juste avant les routes)
app.UseCors("AllowAngular");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Enregistrement de tes routes
app.MapListeDeNaissanceRoutes();
ListeDeNaissance.API.Endpoints.CompteRoutes.MapCompteRoutes(app);

app.Run();