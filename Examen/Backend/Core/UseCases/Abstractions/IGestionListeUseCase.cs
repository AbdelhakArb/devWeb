using System.Collections.Generic;
using System.Threading.Tasks;

namespace ListeDeNaissance.Core.UseCases.Abstractions
{
    public interface IGestionListeUseCase
    {
        Task<IEnumerable<Models.ListeDeNaissance>> ObtenirToutesLesListesAsync();
        Task<Models.ListeDeNaissance?> ObtenirListeParIdAsync(int id);
        Task<IEnumerable<Models.ListeDeNaissance>> ObtenirListesParParentIdAsync(int parentId);
        Task CreerNouvelleListeAsync(Models.ListeDeNaissance list);
        Task ModifierListeExisteAsync(Models.ListeDeNaissance list);
        Task SupprimerListeExisteAsync(int id);
    }
}