using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ListeDeNaissance.Core.UseCases;
using ListeDeNaissance.Core.Models;
using System;
using System.Threading.Tasks;

namespace ListeDeNaissance.API.Endpoints
{
    public static class CompteRoutes
    {
        public static void MapCompteRoutes(this IEndpointRouteBuilder routes)
        {
            // =========================================================================
            // 1. ROUTE D'INSCRIPTION DU PARENT
            // =========================================================================
            routes.MapPost("/api/comptes/inscription-parent", async (CompteParent nouveauParent, IAuthentificationUseCase authUseCase) =>
            {
                try
                {
                    // On passe l'objet directement au UseCase qui gère la logique métier
                    await authUseCase.InscrireParentAsync(nouveauParent);
                    
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
            routes.MapPost("/api/comptes/inscription-visiteur", async (Visiteur nouveauVisiteur, IAuthentificationUseCase authUseCase) =>
            {
                try
                {
                    await authUseCase.InscrireVisiteurAsync(nouveauVisiteur);
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
            // 3. ROUTE DE CONNEXION DU PARENT
            // =========================================================================
            routes.MapPost("/api/comptes/connexion-parent", async (LoginRequest loginData, IAuthentificationUseCase authUseCase) =>
            {
                try
                {
                    // L'API extrait l'email et le mot de passe pour les donner au UseCase
                    var parent = await authUseCase.ConnexionParentAsync(loginData.Email, loginData.MotDePasse);
                    
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
            // 4. ROUTE DE CONNEXION DU VISITEUR
            // =========================================================================
            routes.MapPost("/api/comptes/connexion-visiteur", async (LoginRequest loginData, IAuthentificationUseCase authUseCase) =>
            {
                try
                {
                    var visiteur = await authUseCase.ConnexionVisiteurAsync(loginData.Email, loginData.MotDePasse);
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