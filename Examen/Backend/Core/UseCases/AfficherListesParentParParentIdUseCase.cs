using System.Threading.Tasks;
using ListeDeNaissance.Core.IGateways;
using ListeDeNaissance.Core.Models;
using ListeDeNaissance.Core.UseCases.Abstractions;

namespace ListeDeNaissance.Core.UseCases
{
    public class AfficherListesParentParParentIdUseCase : IAfficherListesParentParParentIdUseCase
    {
        private readonly IListeDeNaissanceGateway _gateway;

        public AfficherListesParentParParentIdUseCase(IListeDeNaissanceGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<IEnumerable<ListeDeNaissance.Core.Models.ListeDeNaissance>> ExecuterAsync(int parentId)
        {
            return await _gateway.ObtenirListesParParentIdAsync(parentId);
        }
    }
}