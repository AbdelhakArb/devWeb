using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ListeDeNaissance.Core.UseCases.Abstractions;
using CoreModels = ListeDeNaissance.Core.Models;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Api.EndPoints
{
    public record SoumettreReservationDto(
        [property: JsonPropertyName("listeDeNaissanceId")] int ListeDeNaissanceId,
        [property: JsonPropertyName("articleId")] int ArticleId,
        [property: JsonPropertyName("qtySouhaitee")] int QtySouhaitee,
        [property: JsonPropertyName("visiteurId")] int VisiteurId,
        [property: JsonPropertyName("messageText")] string? MessageText,
        [property: JsonPropertyName("signatureMessage")] string? SignatureMessage
    );

    public record AjouterArticleDto(
        [property: JsonPropertyName("articleId")] int ArticleId,
        [property: JsonPropertyName("quantite")] int QtySouhaitee
    );

    public record ModifierQuantiteDto(
        [property: JsonPropertyName("articleId")] int ArticleId
    );

    public record ConsultationDto(
        [property: JsonPropertyName("visiteurId")] int VisiteurId
    );

    public record CloturerListeDto(
        [property: JsonPropertyName("statusListe")] string StatusListe
    );

    public static class ListeDeNaissanceRoutes
    {
        public static void MapListeDeNaissanceRoutes(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/listenaissance")
                           .WithTags("ListeDeNaissance");

            group.MapGet("/", async (IAfficherListesParentParParentIdUseCase useCase, int parentId) =>
            {
                var listes = await useCase.ExecuterAsync(parentId);
                return Results.Ok(listes);
            });

            group.MapGet("/parent/{parentId:int}", async (IAfficherListesParentParParentIdUseCase useCase, int parentId) =>
            {
                var listes = await useCase.ExecuterAsync(parentId);
                return Results.Ok(listes);
            });

            group.MapGet("/{id:int}", async (IAfficherListeParIdUseCase useCase, int id) =>
            {
                var liste = await useCase.ExecuterAsync(id);
                return liste is not null ? Results.Ok(liste) : Results.NotFound();
            });

            group.MapGet("/{id:int}/articles", async (IObtenirArticlesListeUseCase useCase, int id) =>
            {
                var articles = await useCase.ExecuterAsync(id);
                return Results.Ok(articles);
            });

            group.MapPost("/", async (ICreerListeDeNaissanceUseCase useCase, CoreModels.ListeDeNaissance liste) =>
            {
                await useCase.ExecuterAsync(liste);
                return Results.Created($"/api/listenaissance/{liste.ListeDeNaissanceId}", liste);
            });

            group.MapPost("/{id:int}/articles", async (IAjouterArticleDansListeUseCase useCase, int id, AjouterArticleDto dto) =>
            {
                await useCase.ExecuterAsync(id, dto.ArticleId, dto.QtySouhaitee);
                return Results.Ok();
            });

            group.MapPost("/{id:int}/articles/incrementer", async (IAjouterArticleDansListeUseCase useCase, int id, ModifierQuantiteDto dto) =>
            {
                await useCase.ExecuterAsync(id, dto.ArticleId, 1);
                return Results.Ok();
            });

            group.MapPost("/{id:int}/articles/decrementer", async (IAjouterArticleDansListeUseCase useCase, int id, ModifierQuantiteDto dto) =>
            {
                await useCase.ExecuterAsync(id, dto.ArticleId, -1);
                return Results.Ok();
            });

            group.MapPost("/{id:int}/consultation", async (IEnregistrerConsultationUseCase useCase, int id, ConsultationDto dto) =>
            {
                await useCase.ExecuterAsync(id, dto.VisiteurId);
                return Results.Ok();
            });

            group.MapPut("/{id:int}/cloturer", async (ICloturerListeUseCase useCase, int id, CloturerListeDto dto) =>
             {
                 await useCase.ExecuterAsync(id, dto.StatusListe);
                 return Results.Ok();
             });



            group.MapPost("/reserver", async (ISoumettreReservationsUseCase useCase, [FromBody] IEnumerable<SoumettreReservationDto> panier) =>
            {
                var modelPanier = new List<CoreModels.ReservationRequestItem>();

                foreach (var p in panier)
                {
                    modelPanier.Add(new CoreModels.ReservationRequestItem
                    {
                        ListeDeNaissanceId = p.ListeDeNaissanceId,
                        ArticleId = p.ArticleId,
                        QtySouhaitee = p.QtySouhaitee,
                        VisiteurId = p.VisiteurId,
                        MessageText = p.MessageText,
                        SignatureMessage = p.SignatureMessage
                    });
                }

                var success = await useCase.ExecuterAsync(modelPanier);
                return success ? Results.Ok(true) : Results.BadRequest(false);
            });
        }
    }
}