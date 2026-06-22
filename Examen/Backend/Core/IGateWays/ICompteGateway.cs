using System.Threading.Tasks;
using ListeDeNaissance.Core.Models;

namespace ListeDeNaissance.Core.IGateways
{
    public interface ICompteGateway
    {
        Task<CompteParent?> ObtenirParentParEmailAsync(string email);
        Task InscrireParentAsync(CompteParent parent); // 🛠️ Harmonisé avec le UseCase
        Task<Visiteur?> ObtenirVisiteurParEmailAsync(string email);
        Task InscrireVisiteurAsync(Visiteur visiteur); // 🛠️ Harmonisé avec le UseCase
        Task<CompteParent?> VerifierConnexionParentAsync(string email, string motDePasseBrut);
        Task<Visiteur?> VerifierConnexionVisiteurAsync(string email, string motDePasseBrut);
    }
}