using System.Threading.Tasks;
using ListeDeNaissance.Core.Models;

namespace ListeDeNaissance.Core.IGateways
{
    public interface ICompteGateway
    {
        Task<CompteParent?> ObtenirParentParEmailAsync(string email);
        Task CreerCompteParentAsync(CompteParent parent);
        Task<Visiteur?> ObtenirVisiteurParEmailAsync(string email);
        Task CreerCompteVisiteurAsync(Visiteur visiteur);

        Task<CompteParent?> VerifierConnexionParentAsync(string email, string motDePasseBrut);
        Task<Visiteur?> VerifierConnexionVisiteurAsync(string email, string motDePasseBrut);
    }
}