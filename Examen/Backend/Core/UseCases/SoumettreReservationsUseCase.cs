using System.Threading.Tasks;
using ListeDeNaissance.Core.Models;
using ListeDeNaissance.Core.IGateways;
using ListeDeNaissance.Core.UseCases.Abstractions;

namespace ListeDeNaissance.Core.UseCases
{
    public class SoumettreReservationsUseCase : ISoumettreReservationsUseCase
    {
        private readonly IListeDeNaissanceGateway _listeGateway;

        public SoumettreReservationsUseCase(IListeDeNaissanceGateway listeGateway)
        {
            _listeGateway = listeGateway;
        }

        public async Task ExecuterAsync(PanierReservationDto panier)
        {
            await _listeGateway.SoumettreReservationsAsync(panier);
        }
    }
}