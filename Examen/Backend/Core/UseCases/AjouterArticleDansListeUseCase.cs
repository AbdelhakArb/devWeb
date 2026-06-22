using System.Threading.Tasks;
using ListeDeNaissance.Core.Models;
using ListeDeNaissance.Core.IGateways;
using ListeDeNaissance.Core.UseCases.Abstractions;

namespace ListeDeNaissance.Core.UseCases
{
    public class AjouterArticleDansListeUseCase : IAjouterArticleDansListeUseCase
    {
        private readonly IListeDeNaissanceGateway _listeGateway;

        public AjouterArticleDansListeUseCase(IListeDeNaissanceGateway listeGateway)
        {
            _listeGateway = listeGateway;
        }

        public async Task ExecuterAsync(PresenceArticleDansListe presenceArticle)
        {
            await _listeGateway.AjouterArticleDansListeAsync(presenceArticle);
        }
    }
}