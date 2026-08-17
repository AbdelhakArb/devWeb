using System.Collections.Generic;
using System.Threading.Tasks;

namespace ListeDeNaissance.Core.IGateways
{
    public interface IListeDeNaissanceGateway
    {
        Task<IEnumerable<Models.ListeDeNaissance>> GetAllAsync();
        Task<Models.ListeDeNaissance?> AfficherListeParIdAsync(int id);
        Task<IEnumerable<Models.ListeDeNaissance>> ObtenirListesParParentIdAsync(int parentId);
        Task InsertAsync(Models.ListeDeNaissance liste);
        Task UpdateAsync(Models.ListeDeNaissance liste);
        Task DeleteAsync(int id);
        Task AjouterArticleDansListeAsync(int listeId, int articleId, int quantite);
        Task MettreAJourQuantiteArticleAsync(int listeId, int articleId, int nouvelleQuantite);
        Task<IEnumerable<Models.PresenceArticleDansListe>> GetArticlesPourReservationRepoAsync(int listeId);
        Task<IEnumerable<Models.Article>> ObtenirArticlesParListeIdAsync(int listeId);
        Task<IEnumerable<Models.Article>> ObtenirCatalogueArticlesAsync();
        Task<int> IncrementerQuantiteArticleAsync(int listeId, int articleId);
        Task<int> DecrementerQuantiteArticleAsync(int listeId, int articleId);
        Task<bool> SoumettreReservationsAsync(IEnumerable<Models.ReservationRequestItem> panier);
        Task EnregistrerConsultationAsync(int listeId, int visiteurId);
        Task CloturerListeAsync(int listeId, string statusListe);
    }
}