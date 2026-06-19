using System;
using System.Threading.Tasks;
using ListeDeNaissance.Core.IGateways;
using ListeDeNaissance.Core.Models;

namespace ListeDeNaissance.Core.UseCases
{
    public class AuthentificationUseCase : IAuthentificationUseCase
    {
        private readonly ICompteGateway _compteGateway;

        public AuthentificationUseCase(ICompteGateway compteGateway)
        {
            _compteGateway = compteGateway;
        }

        // =========================================================================
        // 1. INSCRIPTION
        // =========================================================================
        public async Task InscrireParentAsync(CompteParent parent)
        {
            var compteExistant = await _compteGateway.ObtenirParentParEmailAsync(parent.EmailDeContact);
            if (compteExistant != null)
            {
                throw new InvalidOperationException("Un compte parent existe déjà avec cet email.");
            }

            await _compteGateway.CreerCompteParentAsync(parent);
        }

        public async Task InscrireVisiteurAsync(Visiteur visiteur)
        {
            var visiteurExistant = await _compteGateway.ObtenirVisiteurParEmailAsync(visiteur.VisiteurEmail);
            if (visiteurExistant != null)
            {
                throw new InvalidOperationException("Un compte visiteur existe déjà avec cet email.");
            }

            await _compteGateway.CreerCompteVisiteurAsync(visiteur);
        }

        // =========================================================================
        // 2. CONNEXION (Allégée : On demande à la Gateway de vérifier)
        // =========================================================================
        public async Task<CompteParent> ConnexionParentAsync(string email, string motDePasseBrut)
        {
            // On demande à la gateway de chercher le parent ET de valider son mot de passe en même temps
            var parent = await _compteGateway.VerifierConnexionParentAsync(email, motDePasseBrut);
            
            if (parent == null)
            {
                throw new UnauthorizedAccessException("Identifiants incorrects.");
            }

            return parent;
        }

        public async Task<Visiteur> ConnexionVisiteurAsync(string email, string motDePasseBrut)
        {
            var visiteur = await _compteGateway.VerifierConnexionVisiteurAsync(email, motDePasseBrut);
            
            if (visiteur == null)
            {
                throw new UnauthorizedAccessException("Identifiants incorrects.");
            }

            return visiteur;
        }
    }
}