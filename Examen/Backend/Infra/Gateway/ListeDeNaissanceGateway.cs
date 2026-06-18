using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ListeDeNaissance.Core.IGateways;
using CoreModels = ListeDeNaissance.Core.Models;
using Infra.Repositories.Abstractions;

namespace Infra.Gateway
{
    public class ListeDeNaissanceGateway : IListeDeNaissanceGateway
    {
        private readonly IListeDeNaissanceRepository _repository;

        public ListeDeNaissanceGateway(IListeDeNaissanceRepository repository)
        {
            _repository = repository;
        }

        // =========================================================================
        // MAPPINGS DES LISTES (TRADUCTION DE L'INFRA VERS LE CORE ET INVERSEMENT)
        // =========================================================================

        public async Task<IEnumerable<CoreModels.ListeDeNaissance>> ObtenirToutesLesListesAsync()
        {
            var infraListes = await _repository.GetAllAsync();
            return infraListes.Select(l => new CoreModels.ListeDeNaissance
            {
                // On prend l'IdListeDeNaissance de ton modèle Infra et on le met dans le modèle Core
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

        public async Task EnregistrerListeAsync(CoreModels.ListeDeNaissance liste)
        {
            var infraListe = new Models.ListeDeNaissance
            {
                // MySQL génère généralement l'ID tout seul à l'insertion (Auto-incrément),
                // mais on mappe la propriété pour correspondre à ton modèle Infra
                IdListeDeNaissance = liste.ListeDeNaissanceId, 
                CompteParentId = liste.CompteParentId,
                NomListeDeNaissance = liste.NomListeDeNaissance,
                StatusListe = "Actif" // Valeur par défaut pour éviter le champ vide
            };
            await _repository.InsertAsync(infraListe);
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

        // =========================================================================
        // MÉTHODES POUR LES ARTICLES ET RÉSERVATIONS (Appelées par tes routes)
        // =========================================================================

        public async Task AjouterArticleDansListeAsync(CoreModels.PresenceArticleDansListe presenceArticle)
        {
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<object>> GetArticlesPourReservationAsync(int listeId)
        {
            return await Task.FromResult(new List<object>());
        }

        public async Task<int> IncrementerQuantiteArticleAsync(int listeId, int articleId)
        {
            return await Task.FromResult(1);
        }

        public async Task<int> DecrementerQuantiteArticleAsync(int listeId, int articleId)
        {
            return await Task.FromResult(0);
        }

        public async Task<bool> SoumettreReservationsAsync(CoreModels.PanierReservationDto panier)
        {
            return await Task.FromResult(true);
        }
    }
}