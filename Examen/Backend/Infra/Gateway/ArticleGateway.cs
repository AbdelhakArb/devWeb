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

        // 1. Récupérer le catalogue complet via ton Repository Dapper
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

        // 2. Obtenir un article par son ID
        public async Task<ListeDeNaissance.Core.Models.Article?> ObtenirParIdAsync(int articleId)
        {
            return await Task.FromResult<ListeDeNaissance.Core.Models.Article?>(null);
        }

        // 3. Ajouter l'article à la liste
        public async Task AjouterArticleALaListeAsync(ListeDeNaissance.Core.Models.PresenceArticleDansListe articleDansListe)
        {
            await Task.CompletedTask;
        }

        // 4. Modifier la présence d'un article dans la liste
        public async Task ModifierArticleDansListeAsync(ListeDeNaissance.Core.Models.PresenceArticleDansListe articleDansListe)
        {
            await Task.CompletedTask;
        }

        // 5. Retirer un article de la liste de naissance
        public async Task SupprimerArticleDeLaListeAsync(int presenceArticleId)
        {
            await Task.CompletedTask;
        }

        // 6. Créer une réservation (CORRIGÉ : Reçoit bien une Reservation du Core !)
        public async Task CreerReservationAsync(ListeDeNaissance.Core.Models.Reservation reservation)
        {
            // Si la réservation reçue est nulle, on évite le plantage
            if (reservation == null) return;

            // 🛠️ MAPPING STRICT : Traduction du Core (reservation) vers l'Infra (infraReservation)
            var infraReservation = new Infra.Models.Reservation
            {
                ReservationId = reservation.ReservationId,
                PresenceArticleDansListeId = reservation.PresenceArticleDansListeId,
                
                // On va chercher la quantité dans 'QtyReserve' du Core pour la mettre dans l'Infra
                QuantiteReservee = reservation.QtyReserve,
                
                 NomVisiteur = reservation.Visiteur?.VisiteurNom ?? "Invité Anonyme",
                
                MessageVisiteur = string.Empty,
                StatusReservation = "Validée"
            };

            // On envoie le modèle Infra tout propre au repository Dapper
            _articleRepository.CreateReservation(infraReservation);

            await Task.CompletedTask;
        }

        // 7. Obtenir les détails d'une liaison
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