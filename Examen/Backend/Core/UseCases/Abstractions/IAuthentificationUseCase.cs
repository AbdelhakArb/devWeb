using System.Threading.Tasks;
using ListeDeNaissance.Core.Models;

namespace ListeDeNaissance.Core.UseCases
{
    public interface IAuthentificationUseCase
    {
        // =========================================================================
        // INSCRIPTION
        // =========================================================================
        Task InscrireParentAsync(CompteParent parent);
        Task InscrireVisiteurAsync(Visiteur visiteur);

        // =========================================================================
        // CONNEXION
        // =========================================================================
        Task<CompteParent> ConnexionParentAsync(string email, string motDePasseBrut);
        Task<Visiteur> ConnexionVisiteurAsync(string email, string motDePasseBrut);
    }
}