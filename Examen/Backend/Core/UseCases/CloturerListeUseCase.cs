using System.Threading.Tasks;
using ListeDeNaissance.Core.IGateways;
using ListeDeNaissance.Core.UseCases.Abstractions;

namespace ListeDeNaissance.Core.UseCases
{
    public class CloturerListeUseCase : ICloturerListeUseCase
    {
        private readonly IListeDeNaissanceGateway _listeGateway;

        public CloturerListeUseCase(IListeDeNaissanceGateway gateway)
        {
            _listeGateway = gateway;
        }

        public async Task ExecuterAsync(int listeId, string statusListe)
        {
            await _listeGateway.CloturerListeAsync(listeId, statusListe);
        }
    }
}