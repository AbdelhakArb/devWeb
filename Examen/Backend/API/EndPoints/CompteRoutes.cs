using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ListeDeNaissance.Core.UseCases.Abstractions;
using ListeDeNaissance.Core.Models;
using ListeDeNaissance.API.DTOs;
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
                [FromBody] InscriptionParentRequestDto dto, 
                [FromServices] IInscrireParentUseCase useCase) =>
            {
                try
                {
                    var nouveauParent = new CompteParent
                    {
                        NomPremierParent = dto.Nom,
                        PrenomPremierParent = dto.Prenom,
                        EmailDeContact = dto.Email,
                        MotDePasseCompte = dto.MotDePasse,
                        AdresseParent = dto.Adresse,
                        CpParent = dto.Cp,
                        VilleParent = dto.Ville,
                        PaysParent = dto.Pays
                    };

                    await useCase.ExecuterAsync(nouveauParent);
                    return Results.Json(new { message = "Compte parent créé avec succès !" }, statusCode: 201);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.Json(new { error = ex.Message, details = ex.InnerException?.Message }, statusCode: 500);
                }
            });

            // =========================================================================
            // 2. ROUTE D'INSCRIPTION DU VISITEUR
            // =========================================================================
            group.MapPost("/inscription-visiteur", async (
                [FromBody] InscriptionVisiteurRequestDto dto, 
                [FromServices] IInscrireVisiteurUseCase useCase) =>
            {
                try
                {
                    var nouveauVisiteur = new Visiteur
                    {
                        VisiteurNom = dto.Nom,
                        VisiteurPrenom = dto.Prenom,
                        VisiteurEmail = dto.Email,
                        VisiteurMdp = dto.MotDePasse,
                        VisiteurAdresse = dto.Adresse,
                        VisiteurCP = dto.Cp,
                        VisiteurVille = dto.Ville,
                        VisiteurPays = dto.Pays
                    };

                    await useCase.ExecuterAsync(nouveauVisiteur);
                    return Results.Json(new { message = "Compte visiteur créé avec succès !" }, statusCode: 201);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.Json(new { error = ex.Message, details = ex.InnerException?.Message }, statusCode: 500);
                }
            });

            // =========================================================================
            // 3. ROUTE DE CONNEXION DU PARENT
            // =========================================================================
            group.MapPost("/connexion-parent", async (
                [FromBody] ConnexionRequestDto dto, 
                [FromServices] IConnexionParentUseCase useCase) =>
            {
                try
                {
                    var parent = await useCase.ExecuterAsync(dto.Email, dto.MotDePasse);
                    return Results.Ok(new { message = "Connexion réussie !", utilisateur = parent });
                }
                catch (UnauthorizedAccessException ex)
                {
                    return Results.Json(new { error = ex.Message }, statusCode: 401);
                }
                catch (Exception ex)
                {
                    return Results.Json(new { error = ex.Message }, statusCode: 500);
                }
            });

            // =========================================================================
            // 4. ROUTE DE CONNEXION DU VISITEUR
            // =========================================================================
            group.MapPost("/connexion-visiteur", async (
                [FromBody] ConnexionRequestDto dto, 
                [FromServices] IConnexionVisiteurUseCase useCase) =>
            {
                try
                {
                    var visiteur = await useCase.ExecuterAsync(dto.Email, dto.MotDePasse);
                    return Results.Ok(new { message = "Connexion réussie !", utilisateur = visiteur });
                }
                catch (UnauthorizedAccessException ex)
                {
                    return Results.Json(new { error = ex.Message }, statusCode: 401);
                }
                catch (Exception ex)
                {
                    return Results.Json(new { error = ex.Message }, statusCode: 500);
                }
            });
        }
    }
}