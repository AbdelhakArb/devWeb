using System.Collections.Generic;
using System.Threading.Tasks;

namespace ListeDeNaissance.Core.UseCases.Abstractions
{
    public interface IGestionListeUseCase
    {
        Task<IEnumerable<Models.ListeDeNaissance>> ObtenirToutesAsync();
        Task<Models.ListeDeNaissance?> ObtenirParIdAsync(int id);
        Task<IEnumerable<Models.ListeDeNaissance>> ObtenirParParentIdAsync(int parentId);
        Task CreerAsync(Models.ListeDeNaissance list);
        Task ModifierAsync(Models.ListeDeNaissance list);
        Task SupprimerAsync(int id);
    }
}