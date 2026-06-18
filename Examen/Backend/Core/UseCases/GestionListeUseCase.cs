using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ListeDeNaissance.Core.IGateways;
using ListeDeNaissance.Core.UseCases.Abstractions;

namespace ListeDeNaissance.Core.UseCases
{
    public class GestionListeUseCase : IGestionListeUseCase
    {
        private readonly IListeDeNaissanceGateway _listeGateway;

        public GestionListeUseCase(IListeDeNaissanceGateway listeGateway)
        {
            _listeGateway = listeGateway;
        }

        public async Task<IEnumerable<Models.ListeDeNaissance>> ObtenirToutesLesListesAsync()
        {
            return await _listeGateway.ObtenirToutesLesListesAsync();
        }

        public async Task<Models.ListeDeNaissance?> ObtenirListeParIdAsync(int id)
        {
            return await _listeGateway.ObtenirListeParIdAsync(id);
        }

        public async Task<IEnumerable<Models.ListeDeNaissance>> ObtenirListesParParentIdAsync(int parentId)
        {
            return await _listeGateway.ObtenirListesParParentIdAsync(parentId);
        }

        public async Task CreerNouvelleListeAsync(Models.ListeDeNaissance liste)
        {
            if (string.IsNullOrWhiteSpace(liste.NomListeDeNaissance))
            {
                throw new ArgumentException("Le nom de la liste est obligatoire.");
            }

            await _listeGateway.EnregistrerListeAsync(liste);
        }

        public async Task ModifierListeExisteAsync(Models.ListeDeNaissance liste)
        {
            var listeExistante = await _listeGateway.ObtenirListeParIdAsync(liste.ListeDeNaissanceId);
            if (listeExistante == null)
            {
                throw new KeyNotFoundException("Impossible de modifier : cette liste de naissance n'existe pas.");
            }

            await _listeGateway.ModifierListeAsync(liste);
        }

        public async Task SupprimerListeExisteAsync(int id)
        {
            var listeExistante = await _listeGateway.ObtenirListeParIdAsync(id);
            if (listeExistante == null)
            {
                throw new KeyNotFoundException("Impossible de supprimer : cette liste de naissance n'existe pas.");
            }

            await _listeGateway.SupprimerListeAsync(id);
        }
    }
}