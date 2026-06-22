using System.Collections.Generic;
using System.Threading.Tasks;
using ListeDeNaissance.Core.Models;

namespace ListeDeNaissance.Core.IGateways
{
    public interface IListeDeNaissanceGateway
    {
        // CRUD Listes
        Task EnregistrerListeAsync(ListeDeNaissance.Core.Models.ListeDeNaissance liste);
        Task<IEnumerable<ListeDeNaissance.Core.Models.ListeDeNaissance>> ObtenirToutesLesListesAsync();
        Task<ListeDeNaissance.Core.Models.ListeDeNaissance?> ObtenirListeParIdAsync(int id);
        Task<IEnumerable<ListeDeNaissance.Core.Models.ListeDeNaissance>> ObtenirListesParParentIdAsync(int parentId);
        Task ModifierListeAsync(ListeDeNaissance.Core.Models.ListeDeNaissance liste);
        Task SupprimerListeAsync(int id);

        // Gestion Articles et Réservations
        Task AjouterArticleDansListeAsync(ListeDeNaissance.Core.Models.PresenceArticleDansListe presenceArticle);
        Task<IEnumerable<ListeDeNaissance.Core.Models.PresenceArticleDansListe>> GetArticlesPourReservationAsync(int listeId);
        Task<int> IncrementerQuantiteArticleAsync(int listeId, int articleId);
        Task<int> DecrementerQuantiteArticleAsync(int listeId, int articleId);
        Task<bool> SoumettreReservationsAsync(IEnumerable<PresenceArticleDansListe> panier);
    }
}