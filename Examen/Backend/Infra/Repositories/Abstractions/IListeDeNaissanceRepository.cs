using System.Collections.Generic;
using System.Threading.Tasks;
using Infra.Models; 

namespace Infra.Repositories.Abstractions
{
    public interface IListeDeNaissanceRepository
    {
       Task<IEnumerable<Models.ListeDeNaissance>> GetAllAsync();
        Task<Models.ListeDeNaissance?> GetByIdAsync(int id);
        Task<IEnumerable<Models.ListeDeNaissance>> GetByParentIdAsync(int parentId);
        Task InsertAsync(Models.ListeDeNaissance liste);
        Task UpdateAsync(Models.ListeDeNaissance liste);
        Task DeleteAsync(int id);

        Task AjouterArticleDansListeAsync(int listeId, int articleId, int quantite);

        Task<IEnumerable<Models.PresenceArticleDansListe>> GetArticlesPourReservationRepoAsync(int listeId);
        
        Task<int> IncrementerQuantiteArticleRepoAsync(int listeId, int articleId);
        Task<int> DecrementerQuantiteArticleRepoAsync(int listeId, int articleId);
        
        Task<bool> SoumettreReservationsRepoAsync(IEnumerable<Models.PresenceArticleDansListe> panier);
    }
}