using System.Collections.Generic;
using System.Threading.Tasks;
using ListeDeNaissance.Core.IGateways;
using Infra.Repositories.Abstractions;

namespace Infra.Gateways
{
    public class ListeDeNaissanceGateway : IListeDeNaissanceGateway
    {
        private readonly IListeDeNaissanceRepository _repository;

        public ListeDeNaissanceGateway(IListeDeNaissanceRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ListeDeNaissance.Core.Models.ListeDeNaissance>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<ListeDeNaissance.Core.Models.ListeDeNaissance?> AfficherListeParIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<ListeDeNaissance.Core.Models.ListeDeNaissance>> ObtenirListesParParentIdAsync(int parentId)
        {
            return await _repository.GetByParentIdAsync(parentId);
        }

        public async Task InsertAsync(ListeDeNaissance.Core.Models.ListeDeNaissance liste)
        {
            await _repository.InsertAsync(liste);
        }

        public async Task UpdateAsync(ListeDeNaissance.Core.Models.ListeDeNaissance liste)
        {
            await _repository.UpdateAsync(liste);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task AjouterArticleDansListeAsync(int listeId, int articleId, int quantite)
        {
            await _repository.AjouterArticleDansListeAsync(listeId, articleId, quantite);
        }

        public async Task MettreAJourQuantiteArticleAsync(int listeId, int articleId, int nouvelleQuantite)
        {
            await _repository.MettreAJourQuantiteArticleAsync(listeId, articleId, nouvelleQuantite);
        }

        public async Task<IEnumerable<ListeDeNaissance.Core.Models.PresenceArticleDansListe>> GetArticlesPourReservationRepoAsync(int listeId)
        {
            return await _repository.GetArticlesPourReservationRepoAsync(listeId);
        }

        public async Task<IEnumerable<ListeDeNaissance.Core.Models.Article>> ObtenirArticlesParListeIdAsync(int listeId)
        {
            var infraArticles = await _repository.ObtenirArticlesParListeIdAsync(listeId);
            var coreArticles = new List<ListeDeNaissance.Core.Models.Article>();

            foreach (var item in infraArticles)
            {
                coreArticles.Add(new ListeDeNaissance.Core.Models.Article
                {
                    ArticleId = item.ArticleId,
                    ArticleNom = item.ArticleNom,
                    ArticleDesc = item.ArticleDesc,
                    ArticlePrix = item.ArticlePrix,
                    ArticleQty = item.ArticleQty
                });
            }

            return coreArticles;
        }

        public async Task<IEnumerable<ListeDeNaissance.Core.Models.Article>> ObtenirCatalogueArticlesAsync()
        {
            var infraArticles = await _repository.ObtenirCatalogueArticlesAsync();
            var coreArticles = new List<ListeDeNaissance.Core.Models.Article>();

            foreach (var item in infraArticles)
            {
                coreArticles.Add(new ListeDeNaissance.Core.Models.Article
                {
                    ArticleId = item.ArticleId,
                    ArticleNom = item.ArticleNom,
                    ArticleDesc = item.ArticleDesc,
                    ArticlePrix = item.ArticlePrix,
                    ArticleQty = item.ArticleQty
                });
            }

            return coreArticles;
        }

        public async Task<int> IncrementerQuantiteArticleAsync(int listeId, int articleId)
        {
            return await _repository.IncrementerQuantiteArticleRepoAsync(listeId, articleId);
        }

        public async Task<int> DecrementerQuantiteArticleAsync(int listeId, int articleId)
        {
            return await _repository.DecrementerQuantiteArticleRepoAsync(listeId, articleId);
        }
        /*
        public async Task<bool> SoumettreReservationsAsync(IEnumerable<ListeDeNaissance.Core.Models.ReservationRequestItem> panier)
        {
            return await _repository.SoumettreReservationsRepoAsync(panier);
        }*/

        public async Task<bool> SoumettreReservationsAsync(IEnumerable<ListeDeNaissance.Core.Models.ReservationRequestItem> panier)
        {
            Console.WriteLine("gateway lance");
            return await _repository.SoumettreReservationsRepoAsync(panier);
        }

        public async Task EnregistrerConsultationAsync(int listeId, int visiteurId)
        {
            await _repository.EnregistrerConsultationRepoAsync(listeId, visiteurId);
        }

        public async Task CloturerListeAsync(int listeId, string statusListe)
        {
            await _repository.CloturerListeRepoAsync(listeId, statusListe);
        }
    }
}