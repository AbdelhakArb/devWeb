using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Threading.Tasks;
using ListeDeNaissance.Core.Models;
using ListeDeNaissance.Core.UseCases.Abstractions;

namespace Api.EndPoints
{
    public static class ArticleRoutes
    {
        public static WebApplication AddArticleRoutes(this WebApplication app)
        {
            // On crée un groupe propre pour les articles
            var group = app.MapGroup("api/articles")
                .WithTags("Articles");

            // ROUTE 1 : Obtenir le catalogue d'articles (SOLID - Responsabilité Unique)
            group.MapGet("", async ([FromServices] IObtenirArticlesUseCase obtenirArticlesUseCase) =>
            {
                var articles = await obtenirArticlesUseCase.ObtenirCatalogueArticlesAsync();
                return Results.Ok(articles);
            })
            .WithName("ObtenirCatalogueArticles")
            .Produces<IEnumerable<Article>>(StatusCodes.Status200OK);

            // ROUTE 2 : Créer une réservation (SOLID - On pousse le DTO vers le UseCase dédié)
            group.MapPost("/reservations", async ([FromBody] Reservation reservation, IReserverArticleUseCase useCase) =>
{
    // 1. Tu récupères les IDs nécessaires (depuis le DTO ou le contexte)
    int presenceArticleId = reservation.PresenceArticleDansListeId;
    int quantite = reservation.QtyReserve;
    int visiteurId = reservation.VisiteurId; // 💡 C'est ce paramètre qu'il te manquait !

    // 2. Tu passes les 3 arguments au UseCase
    await useCase.ExecuterAsync(presenceArticleId, quantite, visiteurId);

    return Results.Ok();
})
            .WithName("CreerReservation")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

            return app;
        }
    }
}