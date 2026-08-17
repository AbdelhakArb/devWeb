using System.Threading.Tasks;
using ListeDeNaissance.Core.IGateways;
using ListeDeNaissance.Core.UseCases.Abstractions;

namespace ListeDeNaissance.Core.UseCases
{
    public class EnregistrerConsultationUseCase : IEnregistrerConsultationUseCase
    {
        private readonly IListeDeNaissanceGateway _listeGateway;

        public EnregistrerConsultationUseCase(IListeDeNaissanceGateway gateway)
        {
            _listeGateway = gateway;
        }

        public async Task ExecuterAsync(int listeId, int visiteurId)
        {
            await _listeGateway.EnregistrerConsultationAsync(listeId, visiteurId);
        }
    }
}