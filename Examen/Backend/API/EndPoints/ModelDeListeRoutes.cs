using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ListeDeNaissance.Core.UseCases.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Api.EndPoints
{
    public static class ModelDeListeRoutes
    {
        public static void MapModelDeListeRoutes(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/modeleslistes").WithTags("ModelesListes");

            group.MapGet("/", async ([FromServices] IObtenirTousLesModelsUseCase useCase) =>
            {
                var result = await useCase.ExecuterAsync();
                return Results.Ok(result);
            });

            group.MapGet("/{modelId:int}/articles", async (int modelId, [FromServices] IObtenirArticlesDuModelUseCase useCase) =>
            {
                var result = await useCase.ExecuterAsync(modelId);
                return Results.Ok(result);
            });
        }
    }
}