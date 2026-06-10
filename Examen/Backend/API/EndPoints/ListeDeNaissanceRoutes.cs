using ListeDeNaissance.Core.Usecases.Abstractions;
using CoreModels = ListeDeNaissance.Core.Models;
using ListeDeNaissance.Core.IGateways;
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
            app.MapPost("/api/listedenaissance/article", async (
            CoreModels.PresenceArticleDansListe presenceArticle,
            IListeDeNaissanceGateway listeGateway) =>
        {
            // Sécurité de base : on vérifie que les ID et la quantité tiennent la route
            if (presenceArticle.ListeDeNaissanceId <= 0 || presenceArticle.ArticleId <= 0 || presenceArticle.QtySouhaitee <= 0)
            {
                return Results.BadRequest("Les données fournies sont invalides (ID ou quantité incorrects).");
            }

            await listeGateway.AjouterArticleDansListeAsync(presenceArticle);

            return Results.Ok(new { message = "L'article a bien été ajouté à la liste de naissance !" });
        });
            // Recréation de la route de réservation version C#
            app.MapGet("/api/listedenaissance/{listeId:int}/articles", async (
                int listeId,
                IListeDeNaissanceGateway listeGateway) =>
            {
                var articles = await listeGateway.GetArticlesPourReservationAsync(listeId);
                return Results.Ok(articles);
            });

        }

    }
}