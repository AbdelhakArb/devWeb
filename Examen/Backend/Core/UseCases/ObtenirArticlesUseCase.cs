using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ListeDeNaissance.Core.IGateways;
using ListeDeNaissance.Core.UseCases.Abstractions;
using ListeDeNaissance.Core.Models;

namespace ListeDeNaissance.Core.UseCases
{
    public class ObtenirArticlesUseCase : IObtenirArticlesUseCase
    {
        // 🛠️ Minuscule ici pour respecter les conventions C#
        private readonly IArticleGateway _articleGateway;

        public ObtenirArticlesUseCase(IArticleGateway articleGateway)
        {
            _articleGateway = articleGateway;
        }

        public async Task<IEnumerable<Article>> ObtenirCatalogueArticlesAsync()
        {
            return await _articleGateway.ObtenirCatalogueArticlesAsync();
        }
    }
}