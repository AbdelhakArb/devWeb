using System.Threading.Tasks;
using ListeDeNaissance.Core.IGateways;
using ListeDeNaissance.Core.UseCases.Abstractions;

namespace ListeDeNaissance.Core.UseCases
{
    public class DecrementerArticleListeUseCase : IDecrementerArticleListeUseCase
    {
        private readonly IListeDeNaissanceGateway _listeGateway;

        public DecrementerArticleListeUseCase(IListeDeNaissanceGateway listeGateway)
        {
            _listeGateway = listeGateway;
        }

        public async Task<int> ExecuterAsync(int listeId, int articleId)
        {
            return await _listeGateway.DecrementerQuantiteArticleAsync(listeId, articleId);
        }
    }
}