using ListeDeNaissance.Core.Usecases.Abstractions;
using CoreModels = ListeDeNaissance.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.EndPoints
{
    public static class ListeDeNaissanceRoutes
    {
        public static void MapListeDeNaissanceRoutes(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/listedenaissance", async (
    [FromBody] CoreModels.ListeDeNaissance nouvelleListe,
    ICreerListeDeNaissanceUseCase creerListeUseCase) =>
{
    if (nouvelleListe == null)
    {
        return Results.BadRequest("Données invalides.");
    }

    // ASTUCE : On force une date par défaut pour contourner le blocage MySQL 
    // (Ajoute cette ligne si ta propriété s'appelle datePrevuPourAccouchement)
    // nouvelleListe.datePrevuPourAccouchement = DateTime.Now;

    // Exécution du cas d'utilisation (Core)
    await creerListeUseCase.ExecuterAsync(nouvelleListe);

    return Results.Ok(nouvelleListe);
});
        }
    }
}