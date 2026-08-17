using System.Threading.Tasks;
using ListeDeNaissance.Core.IGateways;
using ListeDeNaissance.Core.Models;
using ListeDeNaissance.Core.UseCases.Abstractions;

namespace ListeDeNaissance.Core.UseCases
{
    public class AfficherListeParIdUseCase : IAfficherListeParIdUseCase
    {
        private readonly IListeDeNaissanceGateway _gateway;

        public AfficherListeParIdUseCase(IListeDeNaissanceGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<ListeDeNaissance.Core.Models.ListeDeNaissance?> ExecuterAsync(int id)
        {
            return await _gateway.AfficherListeParIdAsync(id);
        }
    }
}