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
            // Incrementer la quantité d'un article dans une liste (+ stock magasin -1)
            app.MapPut("/api/listedenaissance/{listeId:int}/articles/{articleId:int}/increment", async (
                int listeId,
                int articleId,
                IListeDeNaissanceGateway listeGateway) =>
            {
                try
                {
                    int nouvelleQty = await listeGateway.IncrementerQuantiteArticleAsync(listeId, articleId);
                    return Results.Ok(new
                    {
                        message = "Quantité incrémentée avec succès !",
                        nouvelleQtySouhaitee = nouvelleQty
                    });
                }
                catch (Exception)
                {
                    return Results.BadRequest("Impossible d'incrémenter l'article. Vérifiez les ID ou les stocks.");
                }
            });
            // Décrémenter la quantité d'un article dans une liste (+ stock magasin +1, supprime si qté = 0)
            app.MapPut("/api/listedenaissance/{listeId:int}/articles/{articleId:int}/decrement", async (
                int listeId,
                int articleId,
                IListeDeNaissanceGateway listeGateway) =>
            {
                try
                {
                    int nouvelleQty = await listeGateway.DecrementerQuantiteArticleAsync(listeId, articleId);

                    return Results.Ok(new
                    {
                        message = nouvelleQty == 0 ? "L'article a été retiré de la liste." : "Quantité décrémentée avec succès !",
                        nouvelleQtySouhaitee = nouvelleQty
                    });
                }
                catch (Exception)
                {
                    return Results.BadRequest("Impossible de décrémenter l'article. Vérifiez que l'article existe bien dans la liste.");
                }
            });
            // Soumettre un panier complet de réservations fait par un visiteur
            app.MapPost("/api/listedenaissance/reserver", async (
                CoreModels.PanierReservationDto panier,
                IListeDeNaissanceGateway listeGateway) =>
            {
                try
                {
                    bool succes = await listeGateway.SoumettreReservationsAsync(panier);
                    return Results.Ok(new { message = "Réservations enregistrées avec succès !" });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            });

        }

    }
}