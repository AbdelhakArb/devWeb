using System;
using System.Threading.Tasks;
using ListeDeNaissance.Core.IGateways;
using ListeDeNaissance.Core.Models;
using ListeDeNaissance.Core.UseCases.Abstractions;

namespace ListeDeNaissance.Core.UseCases
{
    public class CreerListeDeNaissanceUseCase : ICreerListeDeNaissanceUseCase
    {
        private readonly IListeDeNaissanceGateway _listeDeNaissanceGateway;

        public CreerListeDeNaissanceUseCase(IListeDeNaissanceGateway listeDeNaissanceGateway)
        {
            _listeDeNaissanceGateway = listeDeNaissanceGateway;
        }

        public async Task<Models.ListeDeNaissance> ExecuterAsync(Models.ListeDeNaissance nouvelleListe)
        {
            if (nouvelleListe == null)
            {
                throw new ArgumentNullException(nameof(nouvelleListe), "La liste ne peut pas être nulle.");
            }

            if (string.IsNullOrWhiteSpace(nouvelleListe.NomListeDeNaissance))
            {
                throw new ArgumentException("Le nom de la liste de naissance est obligatoire.");
            }

            await _listeDeNaissanceGateway.InsertAsync(nouvelleListe);

            return nouvelleListe;
        }
    }
}