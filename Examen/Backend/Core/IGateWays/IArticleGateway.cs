using ListeDeNaissance.Core.Models;

namespace ListeDeNaissance.Core.IGateways;

public interface IArticleGateway
{
    Task<Article?> ObtenirParIdAsync(int articleId);
    Task<List<Article>> ObtenirTousLesArticlesAsync();
    Task AjouterArticleALaListeAsync(PresenceArticleDansListe articleDansListe);
    Task ModifierArticleDansListeAsync(PresenceArticleDansListe articleDansListe);
    Task SupprimerArticleDeLaListeAsync(int presenceArticleId);
    Task CreerReservationAsync(Reservation reservation);
    Task<PresenceArticleDansListe?> ObtenirPresenceArticleAsync(int presenceArticleId);
}