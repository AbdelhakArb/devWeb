using System.Collections.Generic;
using System.Threading.Tasks;
using ListeDeNaissance.Core.Models;
using ListeDeNaissance.Core.IGateways;
using ListeDeNaissance.Core.UseCases.Abstractions;

namespace ListeDeNaissance.Core.UseCases
{
    public class ObtenirArticlesListeUseCase : IObtenirArticlesListeUseCase
    {
        private readonly IListeDeNaissanceGateway _listeGateway;

        public ObtenirArticlesListeUseCase(IListeDeNaissanceGateway listeGateway)
        {
            _listeGateway = listeGateway;
        }

        public async Task<IEnumerable<PresenceArticleDansListe>> ExecuterAsync(int listeId)
        {
            return await _listeGateway.GetArticlesPourReservationAsync(listeId);
        }
    }
}