using System.Collections.Generic;
using System.Threading.Tasks;
using ListeDeNaissance.Core.Models;

namespace Infra.Repositories.Abstractions
{
    public interface IListeDeNaissanceRepository
    {
        Task<IEnumerable<ListeDeNaissance.Core.Models.ListeDeNaissance>> GetAllAsync();
        Task<ListeDeNaissance.Core.Models.ListeDeNaissance?> GetByIdAsync(int id);
        Task<IEnumerable<ListeDeNaissance.Core.Models.ListeDeNaissance>> GetByParentIdAsync(int parentId);
        Task InsertAsync(ListeDeNaissance.Core.Models.ListeDeNaissance liste);
        Task UpdateAsync(ListeDeNaissance.Core.Models.ListeDeNaissance liste);
        Task DeleteAsync(int id);
        Task AjouterArticleDansListeAsync(int listeId, int articleId, int quantite);
        Task MettreAJourQuantiteArticleAsync(int listeId, int articleId, int nouvelleQuantite);
        Task<IEnumerable<PresenceArticleDansListe>> GetArticlesPourReservationRepoAsync(int listeId);
        Task<IEnumerable<Article>> ObtenirArticlesParListeIdAsync(int listeId);
        Task<IEnumerable<Article>> ObtenirCatalogueArticlesAsync();
        Task<int> IncrementerQuantiteArticleRepoAsync(int listeId, int articleId);
        Task<int> DecrementerQuantiteArticleRepoAsync(int listeId, int articleId);
        Task<bool> SoumettreReservationsRepoAsync(IEnumerable<ReservationRequestItem> panier);
        Task EnregistrerConsultationRepoAsync(int listeId, int visiteurId);
        Task CloturerListeRepoAsync(int listeId, string statusListe);
    }
}