using System.Collections.Generic;
using System.Threading.Tasks;
using ListeDeNaissance.Core.Models;

namespace ListeDeNaissance.Core.IGateways
{
    public interface IArticleGateway
    {
        Task<Core.Models.Article?> ObtenirParIdAsync(int articleId);
        Task<IEnumerable<Core.Models.Article>> ObtenirCatalogueArticlesAsync();
        Task AjouterArticleALaListeAsync(Core.Models.PresenceArticleDansListe articleDansListe);
        Task ModifierArticleDansListeAsync(Core.Models.PresenceArticleDansListe articleDansListe);
        Task SupprimerArticleDeLaListeAsync(int presenceArticleId);
        Task CreerReservationAsync(Core.Models.Reservation reservation);
        Task<Core.Models.PresenceArticleDansListe?> ObtenirPresenceArticleAsync(int presenceArticleId);
    }
}