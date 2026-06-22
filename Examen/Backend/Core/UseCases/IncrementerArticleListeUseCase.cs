using System.Threading.Tasks;
using ListeDeNaissance.Core.IGateways;
using ListeDeNaissance.Core.UseCases.Abstractions;

namespace ListeDeNaissance.Core.UseCases
{
    public class IncrementerArticleListeUseCase : IIncrementerArticleListeUseCase
    {
        private readonly IListeDeNaissanceGateway _listeGateway;

        public IncrementerArticleListeUseCase(IListeDeNaissanceGateway listeGateway)
        {
            _listeGateway = listeGateway;
        }

        public async Task<int> ExecuterAsync(int listeId, int articleId)
        {
            return await _listeGateway.IncrementerQuantiteArticleAsync(listeId, articleId);
        }
    }
}