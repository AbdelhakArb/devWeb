using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ListeDeNaissance.Core.UseCases.Abstractions;
using ListeDeNaissance.Core.Models;
using System;
using System.Threading.Tasks;

namespace ListeDeNaissance.API.Endpoints
{
    public static class CompteRoutes
    {
        public static void MapCompteRoutes(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/comptes")
                              .WithTags("Comptes");

            // =========================================================================
            // 1. ROUTE D'INSCRIPTION DU PARENT
            // =========================================================================
            group.MapPost("/inscription-parent", async (
                [FromBody] CompteParent nouveauParent, 
                [FromServices] IInscrireParentUseCase useCase) =>
            {
                try
                {
                    await useCase.ExecuterAsync(nouveauParent);
                    return Results.Json(new { message = "Compte parent créé avec succès !" }, statusCode: 201);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (Exception)
                {
                    return Results.StatusCode(500);
                }
            });

            // =========================================================================
            // 2. ROUTE D'INSCRIPTION DU VISITEUR
            // =========================================================================
            group.MapPost("/inscription-visiteur", async (
                [FromBody] Visiteur nouveauVisiteur, 
                [FromServices] IInscrireVisiteurUseCase useCase) =>
            {
                try
                {
                    await useCase.ExecuterAsync(nouveauVisiteur);
                    return Results.Json(new { message = "Compte visiteur créé avec succès !" }, statusCode: 201);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (Exception)
                {
                    return Results.StatusCode(500);
                }
            });

            // =========================================================================
            // 3. ROUTE DE CONNEXION DU PARENT (Renvoie l'objet Parent directement)
            // =========================================================================
            group.MapPost("/connexion-parent", async (
                [FromBody] LoginRequest loginData, 
                [FromServices] IConnexionParentUseCase useCase) =>
            {
                try
                {
                    var parent = await useCase.ExecuterAsync(loginData.Email, loginData.MotDePasse);
                    return Results.Ok(new { message = "Connexion réussie !", utilisateur = parent });
                }
                catch (UnauthorizedAccessException ex)
                {
                    return Results.Json(new { error = ex.Message }, statusCode: 401);
                }
                catch (Exception)
                {
                    return Results.StatusCode(500);
                }
            });

            // =========================================================================
            // 4. ROUTE DE CONNEXION DU VISITEUR (Renvoie l'objet Visiteur directement)
            // =========================================================================
            group.MapPost("/connexion-visiteur", async (
                [FromBody] LoginRequest loginData, 
                [FromServices] IConnexionVisiteurUseCase useCase) =>
            {
                try
                {
                    var visiteur = await useCase.ExecuterAsync(loginData.Email, loginData.MotDePasse);
                    return Results.Ok(new { message = "Connexion réussie !", utilisateur = visiteur });
                }
                catch (UnauthorizedAccessException ex)
                {
                    return Results.Json(new { error = ex.Message }, statusCode: 401);
                }
                catch (Exception)
                {
                    return Results.StatusCode(500);
                }
            });
        }
    }

    public record LoginRequest(string Email, string MotDePasse);
}