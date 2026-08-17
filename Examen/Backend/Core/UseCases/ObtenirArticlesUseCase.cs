using System.Collections.Generic;
using System.Threading.Tasks;
using ListeDeNaissance.Core.IGateways;
using ListeDeNaissance.Core.Models;
using ListeDeNaissance.Core.UseCases.Abstractions;

namespace ListeDeNaissance.Core.UseCases
{
    public class ObtenirArticlesUseCase : IObtenirArticlesUseCase
    {
        private readonly IListeDeNaissanceGateway _gateway;

        public ObtenirArticlesUseCase(IListeDeNaissanceGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<IEnumerable<Article>> ObtenirCatalogueArticlesAsync()
        {
            return await _gateway.ObtenirCatalogueArticlesAsync();
        }
    }
}