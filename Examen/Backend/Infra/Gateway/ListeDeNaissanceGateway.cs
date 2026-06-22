using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ListeDeNaissance.Core.IGateways;
using CoreModels = ListeDeNaissance.Core.Models;
using Infra.Repositories.Abstractions;
using Infra.Models;

namespace Infra.Gateway
{
    public class ListeDeNaissanceGateway : IListeDeNaissanceGateway
    {
        private readonly IListeDeNaissanceRepository _repository;

        public ListeDeNaissanceGateway(IListeDeNaissanceRepository repository)
        {
            _repository = repository;
        }

        // 1. MÉTHODE CRÉATION (Le nom dans ton interface semble être AddListeDeNaissanceAsync)
        public async Task AddListeDeNaissanceAsync(CoreModels.ListeDeNaissance liste)
        {
            var infraListe = new Models.ListeDeNaissance
            {
                IdListeDeNaissance = liste.ListeDeNaissanceId,
                CompteParentId = liste.CompteParentId,
                NomListeDeNaissance = liste.NomListeDeNaissance,
                StatusListe = "Actif"
            };
            await _repository.InsertAsync(infraListe);
        }

        // 2. MÉTHODE AJOUTER (Renommée pour correspondre à Enregistrer si besoin)
        public async Task EnregistrerListeAsync(CoreModels.ListeDeNaissance liste)
        {
            await AddListeDeNaissanceAsync(liste);
        }

        // =========================================================================
        // AUTRES MAPPINGS (Identiques à ton code)
        // =========================================================================

        public async Task<IEnumerable<CoreModels.ListeDeNaissance>> ObtenirToutesLesListesAsync()
        {
            var infraListes = await _repository.GetAllAsync();
            return infraListes.Select(l => new CoreModels.ListeDeNaissance
            {
                ListeDeNaissanceId = l.IdListeDeNaissance,
                CompteParentId = l.CompteParentId,
                NomListeDeNaissance = l.NomListeDeNaissance
            });
        }

        public async Task<CoreModels.ListeDeNaissance?> ObtenirListeParIdAsync(int id)
        {
            var l = await _repository.GetByIdAsync(id);
            if (l == null) return null;

            return new CoreModels.ListeDeNaissance
            {
                ListeDeNaissanceId = l.IdListeDeNaissance,
                CompteParentId = l.CompteParentId,
                NomListeDeNaissance = l.NomListeDeNaissance
            };
        }

        public async Task<IEnumerable<CoreModels.ListeDeNaissance>> ObtenirListesParParentIdAsync(int parentId)
        {
            var infraListes = await _repository.GetByParentIdAsync(parentId);
            return infraListes.Select(l => new CoreModels.ListeDeNaissance
            {
                ListeDeNaissanceId = l.IdListeDeNaissance,
                CompteParentId = l.CompteParentId,
                NomListeDeNaissance = l.NomListeDeNaissance
            });
        }

        public async Task ModifierListeAsync(CoreModels.ListeDeNaissance liste)
        {
            var infraListe = new Models.ListeDeNaissance
            {
                IdListeDeNaissance = liste.ListeDeNaissanceId,
                CompteParentId = liste.CompteParentId,
                NomListeDeNaissance = liste.NomListeDeNaissance
            };
            await _repository.UpdateAsync(infraListe);
        }

        public async Task SupprimerListeAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task AjouterArticleDansListeAsync(CoreModels.PresenceArticleDansListe presenceArticle)
        {
            await _repository.AjouterArticleDansListeAsync(presenceArticle.ListeDeNaissanceId, presenceArticle.ArticleId, presenceArticle.QtySouhaitee);
        }

        public async Task<IEnumerable<CoreModels.PresenceArticleDansListe>> GetArticlesPourReservationAsync(int listeId)
        {
            var infraArticles = await _repository.GetArticlesPourReservationRepoAsync(listeId);

            return infraArticles.Select(a => new CoreModels.PresenceArticleDansListe
            {
                ListeDeNaissanceId = a.ListeDeNaissanceId,
                ArticleId = a.ArticleId,
                QtySouhaitee = a.QtySouhaitee
            });
        }

        public async Task<int> IncrementerQuantiteArticleAsync(int listeId, int articleId)
        {
            return await _repository.IncrementerQuantiteArticleRepoAsync(listeId, articleId);
        }

        public async Task<int> DecrementerQuantiteArticleAsync(int listeId, int articleId)
        {
            return await _repository.DecrementerQuantiteArticleRepoAsync(listeId, articleId);
        }

        public async Task<bool> SoumettreReservationsAsync(IEnumerable<CoreModels.PresenceArticleDansListe> panier)
        {
            var infraPanier = panier.Select(item => new Infra.Models.PresenceArticleDansListe
            {
                ListeDeNaissanceId = item.ListeDeNaissanceId,
                ArticleId = item.ArticleId,
                QtySouhaitee = item.QtySouhaitee
            });
            return await _repository.SoumettreReservationsRepoAsync(infraPanier);
        }
    }
}