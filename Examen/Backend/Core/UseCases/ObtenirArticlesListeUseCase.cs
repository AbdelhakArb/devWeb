using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using ListeDeNaissance.Core.Models;
using ListeDeNaissance.Core.IGateways;
using ListeDeNaissance.Core.UseCases.Abstractions;

namespace ListeDeNaissance.Core.UseCases
{
    public class ObtenirArticlesListeUseCase : IObtenirArticlesListeUseCase
    {
        private readonly IListeDeNaissanceGateway _listeGateway;

        public ObtenirArticlesListeUseCase(IListeDeNaissanceGateway listeGateway)
        {
            _listeGateway = listeGateway;
        }

        public async Task<IEnumerable<Article>> ExecuterAsync(int listeId)
        {
            var articles = await _listeGateway.ObtenirArticlesParListeIdAsync(listeId);

            System.Console.WriteLine($"[DEBUG USECASE] Liste ID {listeId} -> {articles?.Count() ?? 0} articles récupérés.");

            return articles ?? Enumerable.Empty<Article>();
        }
    }
}