using System.Collections.Generic;
using System.Threading.Tasks;
using ListeDeNaissance.Core.IGateways;
using ListeDeNaissance.Core.Models;
using ListeDeNaissance.Core.UseCases.Abstractions;

namespace ListeDeNaissance.Core.UseCases
{
    public class SoumettreReservationsUseCase : ISoumettreReservationsUseCase
    {
        private readonly IListeDeNaissanceGateway _gateway;

        public SoumettreReservationsUseCase(IListeDeNaissanceGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<bool> ExecuterAsync(IEnumerable<PresenceArticleDansListe> panier)
        {
            if (panier == null) return false;
            
            return await _gateway.SoumettreReservationsAsync(panier);
        }
    }
}