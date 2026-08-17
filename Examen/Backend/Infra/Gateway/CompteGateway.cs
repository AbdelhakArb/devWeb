using System.Threading.Tasks;
using ListeDeNaissance.Core.IGateways;
using ListeDeNaissance.Core.Models;
using Infra.Repositories.Abstractions;

namespace Infra.Gateway
{
    public class CompteGateway : ICompteGateway
    {
        private readonly ICompteRepository _compteRepository;

        public CompteGateway(ICompteRepository compteRepository)
        {
            _compteRepository = compteRepository;
        }

        public async Task<CompteParent?> ObtenirParentParEmailAsync(string email) => await _compteRepository.GetCompteByEmailAsync(email);
        public async Task<Visiteur?> ObtenirVisiteurParEmailAsync(string email) => await _compteRepository.GetVisiteurByEmailAsync(email);

        public async Task InscrireParentAsync(CompteParent parent)
        {
            await _compteRepository.CreateCompteAsync(parent);
        }

        public async Task InscrireVisiteurAsync(Visiteur visiteur)
        {
            await _compteRepository.CreateVisiteurAsync(visiteur);
        }

        public async Task<CompteParent?> VerifierConnexionParentAsync(string email, string motDePasseBrut)
        {
            var parent = await _compteRepository.GetCompteByEmailAsync(email);
            if (parent != null && BCrypt.Net.BCrypt.Verify(motDePasseBrut, parent.MotDePasseCompte))
            {
                return parent; 
            }
            return null; 
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