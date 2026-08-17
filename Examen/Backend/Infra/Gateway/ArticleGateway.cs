using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ListeDeNaissance.Core.IGateways;
using Infra.Repositories.Abstractions;
using ListeDeNaissance.Core.Models;

namespace Infra.Gateway
{
    public class ArticleGateway : IArticleGateway
    {
        private readonly IArticleRepository _articleRepository;

        public ArticleGateway(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<IEnumerable<ListeDeNaissance.Core.Models.Article>> ObtenirCatalogueArticlesAsync()  
        {
            var infraArticles = await _articleRepository.ObtenirTousLesArticlesAsync();
            return infraArticles.Select(a => new ListeDeNaissance.Core.Models.Article
            {
                ArticleId = a.ArticleId,      
                ArticleNom = a.ArticleNom,    
                ArticleDesc = a.ArticleDesc,   
                ArticleQty = a.ArticleQty,    
                ArticlePrix = a.ArticlePrix,   

                CategorieArticles = new List<ListeDeNaissance.Core.Models.CategorieArticle>(),
                PresenceDansListes = new List<ListeDeNaissance.Core.Models.PresenceArticleDansListe>(),
                PresenceDansModels = new List<ListeDeNaissance.Core.Models.PresenceArticleDansModel>()
            });
        }

        public async Task<ListeDeNaissance.Core.Models.Article?> ObtenirParIdAsync(int articleId)
        {
            var infraArticle = await _articleRepository.ObtenirArticleParIdAsync(articleId);
            if (infraArticle == null) return null;

            return new ListeDeNaissance.Core.Models.Article
            {
                ArticleId = infraArticle.ArticleId,
                ArticleNom = infraArticle.ArticleNom,
                ArticleDesc = infraArticle.ArticleDesc,
                ArticleQty = infraArticle.ArticleQty,
                ArticlePrix = infraArticle.ArticlePrix
            };
        }

        public async Task AjouterArticleALaListeAsync(ListeDeNaissance.Core.Models.PresenceArticleDansListe articleDansListe)
        {
            await Task.CompletedTask;
        }

        public async Task ModifierArticleDansListeAsync(ListeDeNaissance.Core.Models.PresenceArticleDansListe articleDansListe)
        {
            await Task.CompletedTask;
        }

        public async Task SupprimerArticleDeLaListeAsync(int presenceArticleId)
        {
            await Task.CompletedTask;
        }

        public async Task CreerReservationAsync(ListeDeNaissance.Core.Models.Reservation reservation)
        {
            if (reservation == null) return;

            var infraReservation = new Infra.Models.Reservation
            {
                ReservationId = reservation.ReservationId,
                PresenceArticleDansListeId = reservation.PresenceArticleDansListeId,
                QuantiteReservee = reservation.QtyReserve,
                NomVisiteur = reservation.Visiteur?.VisiteurNom ?? "Invité Anonyme",
                MessageVisiteur = string.Empty,
                StatusReservation = "Validée"
            };

            _articleRepository.CreateReservation(infraReservation);

            await Task.CompletedTask;
        }

        public async Task<ListeDeNaissance.Core.Models.PresenceArticleDansListe?> ObtenirPresenceArticleAsync(int presenceArticleId)
        {
            var infraPresence = _articleRepository.GetPresenceArticleById(presenceArticleId);

            if (infraPresence == null) return null;

            var corePresence = new ListeDeNaissance.Core.Models.PresenceArticleDansListe
            {
                PresenceArticleDansListeId = infraPresence.PresenceArticleDansListeId,
                ListeDeNaissanceId = infraPresence.ListeDeNaissanceId,
                ArticleId = infraPresence.ArticleId,
                QtySouhaitee = infraPresence.QtySouhaitee,
                ListeDeNaissance = null!,
                Article = null!,
                Reservations = new List<ListeDeNaissance.Core.Models.Reservation>()
            };

            return await Task.FromResult(corePresence);
        }
    }
}