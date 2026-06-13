using Infra.Repositories.Abstractions;
using ListeDeNaissance.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace ListeDeNaissance.API.Endpoints;

public static class CompteRoutes
{
    public static void MapCompteRoutes(this IEndpointRouteBuilder app)
    {
        // =========================================================================
        // 1. LES INSCRIPTIONS (Toujours séparées car le choix du rôle est explicite)
        // =========================================================================

        // --- INSCRIPTION PARENT ---
        app.MapPost("/api/auth/parent/register", ([FromBody] CompteParent nouveauParent, ICompteRepository compteRepo) =>
        {
            try
            {
                var compteExistant = compteRepo.GetCompteByEmail(nouveauParent.EmailDeContact);
                if (compteExistant != null)
                {
                    return Results.BadRequest(new { error = "Cet email est déjà utilisé par un parent." });
                }

                compteRepo.CreateCompte(nouveauParent);
                return Results.Ok(new { message = "Compte parent créé avec succès !" });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        // --- INSCRIPTION VISITEUR ---
        app.MapPost("/api/auth/visiteur/register", ([FromBody] Visiteur nouveauVisiteur, ICompteRepository compteRepo) =>
        {
            try
            {
                var existant = compteRepo.GetVisiteurByEmail(nouveauVisiteur.VisiteurEmail);
                if (existant != null)
                {
                    return Results.BadRequest(new { error = "Cet email est déjà utilisé par un visiteur." });
                }

                compteRepo.CreateVisiteur(nouveauVisiteur);
                return Results.Ok(new { message = "Compte visiteur créé avec succès !" });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });


        // =========================================================================
        // 2. LA CONNEXION UNIQUE (OPTION B - RECHERCHE INTELLIGENTE)
        // =========================================================================
        
        app.MapPost("/api/auth/login", ([FromBody] LoginRequestDto loginRequest, ICompteRepository compteRepo) =>
        {
            // --- TENTATIVE 1 : On cherche dans les PARENTS ---
            var compteParent = compteRepo.GetCompteByEmail(loginRequest.Email);
            
            if (compteParent != null)
            {
                // On vérifie le mot de passe du parent
                bool mdpParentCorrect = BCrypt.Net.BCrypt.Verify(loginRequest.Password, compteParent.MotDePasseCompte);

                if (mdpParentCorrect)
                {
                    return Results.Ok(new
                    {
                        id = compteParent.CompteParentId,
                        email = compteParent.EmailDeContact,
                        nom = compteParent.NomPremierParent,
                        prenom = compteParent.PrenomPremierParent,
                        role = "parent", // Le Front-end sait instantanément que c'est un parent !
                        message = "Connexion réussie en tant que Parent !"
                    });
                }
                
                // Si l'email existe chez les parents mais que le MDP est faux, on s'arrête ici
                return Results.Json(new { error = "Identifiants invalides." }, statusCode: 401);
            }

            // --- TENTATIVE 2 : Si ce n'est pas un parent, on cherche dans les VISITEURS ---
            var visiteur = compteRepo.GetVisiteurByEmail(loginRequest.Email);
            
            if (visiteur != null)
            {
                // On vérifie le mot de passe du visiteur
                bool mdpVisiteurCorrect = BCrypt.Net.BCrypt.Verify(loginRequest.Password, visiteur.VisiteurMdp);

                if (mdpVisiteurCorrect)
                {
                    return Results.Ok(new
                    {
                        id = visiteur.VisiteurId,
                        email = visiteur.VisiteurEmail,
                        nom = visiteur.VisiteurNom,
                        prenom = visiteur.VisiteurPrenom,
                        role = "visiteur", // Le Front-end sait instantanément que c'est un visiteur !
                        message = "Connexion réussie en tant que Visiteur !"
                    });
                }

                // Si l'email existe chez les visiteurs mais que le MDP est faux
                return Results.Json(new { error = "Identifiants invalides." }, statusCode: 401);
            }

            // --- TENTATIVE 3 : L'email n'existe nulle part ---
            return Results.Json(new { error = "Identifiants invalides." }, statusCode: 401);
        });
    }
}