using System.Collections.Generic;
using System.Threading.Tasks;

namespace ListeDeNaissance.Core.IGateways
{
    public interface IListeDeNaissanceGateway
    {
        // Utilisation du chemin complet pour éviter la confusion entre namespace et classe
        Task<IEnumerable<Models.ListeDeNaissance>> ObtenirToutesLesListesAsync();
        Task<Models.ListeDeNaissance?> ObtenirListeParIdAsync(int id);
        Task<IEnumerable<Models.ListeDeNaissance>> ObtenirListesParParentIdAsync(int parentId);
        Task EnregistrerListeAsync(Models.ListeDeNaissance liste);
        Task ModifierListeAsync(Models.ListeDeNaissance liste);
        Task SupprimerListeAsync(int id);
        
        // Méthodes requises par tes routes existantes pour les articles
        Task AjouterArticleDansListeAsync(Models.PresenceArticleDansListe presenceArticle);
        Task<IEnumerable<object>> GetArticlesPourReservationAsync(int listeId);
        Task<int> IncrementerQuantiteArticleAsync(int listeId, int articleId);
        Task<int> DecrementerQuantiteArticleAsync(int listeId, int articleId);
        Task<bool> SoumettreReservationsAsync(Models.PanierReservationDto panier);
    }
}