using ListeDeNaissance.Core.Usecases.Abstractions;
using CoreModels = ListeDeNaissance.Core.Models;
using ListeDeNaissance.Core.IGateways;
using Microsoft.AspNetCore.Mvc;

namespace Api.EndPoints
{
    // --- LES DTOS POUR FAIRE PLAISIR AU PROF ---
    public record CreerListeDto(int CompteParentId, string NomListeDeNaissance);
    public record AjouterArticleDto(int ListeDeNaissanceId, int ArticleId, int QtySouhaitee);

    public static class ListeDeNaissanceRoutes
    {
        public static void MapListeDeNaissanceRoutes(this IEndpointRouteBuilder app)
        {
            // --- 1. CRÉER UNE LISTE (Utilise CreerListeDto) ---
            app.MapPost("/api/listedenaissance", async (
                [FromBody] CreerListeDto dto,
                ICreerListeDeNaissanceUseCase creerListeUseCase) =>
            {
                if (dto == null || string.IsNullOrWhiteSpace(dto.NomListeDeNaissance))
                {
                    return Results.BadRequest("Données de liste invalides.");
                }

                // On convertit le DTO vers le modèle attendu par le UseCase
                var nouvelleListe = new CoreModels.ListeDeNaissance
                {
                    CompteParentId = dto.CompteParentId,
                    NomListeDeNaissance = dto.NomListeDeNaissance
                };

                await creerListeUseCase.ExecuterAsync(nouvelleListe);

                return Results.Ok(nouvelleListe);
            });

            // --- 2. AJOUTER UN ARTICLE DANS UNE LISTE (Utilise AjouterArticleDto) ---
            app.MapPost("/api/listedenaissance/article", async (
                [FromBody] AjouterArticleDto dto,
                IListeDeNaissanceGateway listeGateway) =>
            {
                if (dto == null || dto.ListeDeNaissanceId <= 0 || dto.ArticleId <= 0 || dto.QtySouhaitee <= 0)
                {
                    return Results.BadRequest("Les données fournies sont invalides (ID ou quantité incorrects).");
                }

                // On mappe vers l'objet métier PresenceArticleDansListe
                var presenceArticle = new CoreModels.PresenceArticleDansListe
                {
                    ListeDeNaissanceId = dto.ListeDeNaissanceId,
                    ArticleId = dto.ArticleId,
                    QtySouhaitee = dto.QtySouhaitee
                };

                await listeGateway.AjouterArticleDansListeAsync(presenceArticle);

                return Results.Ok(new { message = "L'article a bien été ajouté à la liste de naissance !" });
            });

            // --- 3. RÉCUPÉRER LES ARTICLES POUR RÉSERVATION ---
            app.MapGet("/api/listedenaissance/{listeId:int}/articles", async (
                int listeId,
                IListeDeNaissanceGateway listeGateway) =>
            {
                var articles = await listeGateway.GetArticlesPourReservationAsync(listeId);
                return Results.Ok(articles);
            });

            // --- 4. INCREMENTER QUANTITÉ ---
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

            // --- 5. DÉCRÉMENTER QUANTITÉ ---
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

            // --- 6. SOUMETTRE LE PANIER DE RÉSERVATION VISITEUR ---
            app.MapPost("/api/listedenaissance/reserver", async (
                [FromBody] CoreModels.PanierReservationDto panier,
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