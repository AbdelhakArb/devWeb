using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ListeDeNaissance.Core.UseCases.Abstractions;
using CoreModels = ListeDeNaissance.Core.Models;
using System;

namespace Api.EndPoints
{
    // --- LES DTOS POUR FAIRE PLAISIR AU PROF ---
    public record CreerListeDto(int CompteParentId, string NomListeDeNaissance);
    public record AjouterArticleDto(int ListeDeNaissanceId, int ArticleId, int QtySouhaitee);

    public static class ListeDeNaissanceRoutes
    {
        public static void MapListeDeNaissanceRoutes(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/listedenaissance")
                           .WithTags("ListeDeNaissance");

            // --- 1. CRÉER UNE LISTE ---
            group.MapPost("", async (
                [FromBody] CreerListeDto dto,
                [FromServices] ICreerListeDeNaissanceUseCase useCase) =>
            {
                if (dto == null || string.IsNullOrWhiteSpace(dto.NomListeDeNaissance))
                {
                    return Results.BadRequest("Données de liste invalides.");
                }

                var nouvelleListe = new CoreModels.ListeDeNaissance
                {
                    CompteParentId = dto.CompteParentId,
                    NomListeDeNaissance = dto.NomListeDeNaissance
                };

                await useCase.ExecuterAsync(nouvelleListe);
                return Results.Ok(nouvelleListe);
            });

            // --- 2. AJOUTER UN ARTICLE DANS UNE LISTE ---
            group.MapPost("/article", async (
                [FromBody] AjouterArticleDto dto,
                [FromServices] IAjouterArticleDansListeUseCase useCase) =>
            {
                if (dto == null || dto.ListeDeNaissanceId <= 0 || dto.ArticleId <= 0 || dto.QtySouhaitee <= 0)
                {
                    return Results.BadRequest("Les données fournies sont invalides (ID ou quantité incorrects).");
                }

                var presenceArticle = new CoreModels.PresenceArticleDansListe
                {
                    ListeDeNaissanceId = dto.ListeDeNaissanceId,
                    ArticleId = dto.ArticleId,
                    QtySouhaitee = dto.QtySouhaitee
                };

                await useCase.ExecuterAsync(presenceArticle);
                return Results.Ok(new { message = "L'article a bien été ajouté à la liste de naissance !" });
            });

            // --- 3. RÉCUPÉRER LES ARTICLES POUR RÉSERVATION ---
            group.MapGet("/{listeId:int}/articles", async (
                int listeId,
                [FromServices] IObtenirArticlesListeUseCase useCase) =>
            {
                var articles = await useCase.ExecuterAsync(listeId);
                return Results.Ok(articles);
            });

            // --- 4. INCREMENTER QUANTITÉ ---
            group.MapPut("/{listeId:int}/articles/{articleId:int}/increment", async (
                int listeId,
                int articleId,
                [FromServices] IIncrementerArticleListeUseCase useCase) =>
            {
                try
                {
                    int nouvelleQty = await useCase.ExecuterAsync(listeId, articleId);
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
            group.MapPut("/{listeId:int}/articles/{articleId:int}/decrement", async (
                int listeId,
                int articleId,
                [FromServices] IDecrementerArticleListeUseCase useCase) =>
            {
                try
                {
                    int nouvelleQty = await useCase.ExecuterAsync(listeId, articleId);

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
            group.MapPost("/reserver", async (
                [FromBody] CoreModels.PanierReservationDto panier,
                [FromServices] ISoumettreReservationsUseCase useCase) =>
            {
                try
                {
                    await useCase.ExecuterAsync(panier);
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