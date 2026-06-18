using System.Threading.Tasks;
using ListeDeNaissance.Core.IGateways;
using ListeDeNaissance.Core.Models;
using Infra.Repositories.Abstractions;
using BCrypt.Net; // Utilisable ici car installé dans l'Infra !

namespace Infra.Gateway
{
    public class CompteGateway : ICompteGateway
    {
        private readonly ICompteRepository _compteRepository;

        public CompteGateway(ICompteRepository compteRepository)
        {
            _compteRepository = compteRepository;
        }

        public async Task<CompteParent?> ObtenirParentParEmailAsync(string email)
        {
            return await _compteRepository.GetCompteByEmailAsync(email);
        }

        public async Task CreerCompteParentAsync(CompteParent parent)
        {
            await _compteRepository.CreateCompteAsync(parent);
        }

        public async Task<Visiteur?> ObtenirVisiteurParEmailAsync(string email)
        {
            return await _compteRepository.GetVisiteurByEmailAsync(email);
        }

        public async Task CreerCompteVisiteurAsync(Visiteur visiteur)
        {
            await _compteRepository.CreateVisiteurAsync(visiteur);
        }

        // =========================================================================
        // IMPLÉMENTATION DE LA VÉRIFICATION SÉCURISÉE (AVEC BCRYPT)
        // =========================================================================
        
        public async Task<CompteParent?> VerifierConnexionParentAsync(string email, string motDePasseBrut)
        {
            var parent = await _compteRepository.GetCompteByEmailAsync(email);
            
            // Si le parent existe, on utilise BCrypt pour comparer le mot de passe
            if (parent != null && BCrypt.Net.BCrypt.Verify(motDePasseBrut, parent.MotDePasseCompte))
            {
                return parent; // Connexion réussie
            }
            
            return null; // Identifiants invalides
        }

        public async Task<Visiteur?> VerifierConnexionVisiteurAsync(string email, string motDePasseBrut)
        {
            var visiteur = await _compteRepository.GetVisiteurByEmailAsync(email);
            
            if (visiteur != null && BCrypt.Net.BCrypt.Verify(motDePasseBrut, visiteur.VisiteurMdp))
            {
                return visiteur;
            }
            
            return null;
        }
    }
}