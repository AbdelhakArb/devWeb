using ListeDeNaissance.Core.Models;

namespace ListeDeNaissance.Core.IGateways;

public interface IListeDeNaissanceGateway
{
    Task CreerListeAsync(Models.ListeDeNaissance liste);
    Task<Models.ListeDeNaissance?> ObtenirListeParIdAsync(int listeId);
    Task<List<Models.ListeDeNaissance>> ObtenirListesParParentIdAsync(int compteParentId);
    Task<Models.ListeDeNaissance?> ObtenirListeParCodeOuLienAsync(string code);
    Task<List<ModelDeListeDeNaissance>> ObtenirTousLesModelesAsync();
    Task AjouterArticleDansListeAsync(PresenceArticleDansListe presenceArticle);
    Task<List<ListeDeNaissance.Core.Models.ArticleItemInListe>> GetArticlesPourReservationAsync(int listeId);
    Task<int> IncrementerQuantiteArticleAsync(int listeId, int articleId);
    Task<int> DecrementerQuantiteArticleAsync(int listeId, int articleId);
    Task<bool> SoumettreReservationsAsync(PanierReservationDto panier);
    
}
