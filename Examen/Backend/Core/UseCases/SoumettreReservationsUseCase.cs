using System.Collections.Generic;
using System.Threading.Tasks;
using ListeDeNaissance.Core.IGateways;
using ListeDeNaissance.Core.Models;
using ListeDeNaissance.Core.UseCases.Abstractions;

namespace ListeDeNaissance.Core.UseCases
{
    public class SoumettreReservationsUseCase : ISoumettreReservationsUseCase
    {
        private readonly IListeDeNaissanceGateway _listeDeNaissanceGateway;

        public SoumettreReservationsUseCase(IListeDeNaissanceGateway listeDeNaissanceGateway)
        {
           _listeDeNaissanceGateway = listeDeNaissanceGateway;
        }

        public async Task<bool> ExecuterAsync(IEnumerable<ReservationRequestItem> panier)
        {
            Console.WriteLine("usecase lance");
            
            if (panier == null) return false;
            return await _listeDeNaissanceGateway.SoumettreReservationsAsync(panier);
        }
    }
}