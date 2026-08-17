using System.Collections.Generic;
using System.Threading.Tasks;
using ListeDeNaissance.Core.IGateways;
using ListeDeNaissance.Core.Models;
using ListeDeNaissance.Core.UseCases.Abstractions;

namespace ListeDeNaissance.Core.UseCases
{
    public class ObtenirTousLesModelsUseCase : IObtenirTousLesModelsUseCase
    {
        private readonly IModelDeListeGateway _gateway;
        public ObtenirTousLesModelsUseCase(IModelDeListeGateway gateway) => _gateway = gateway;
        public async Task<IEnumerable<ModelDeListeDeNaissance>> ExecuterAsync() => await _gateway.ObtenirTousLesModelsAsync();
    }

    public class ObtenirArticlesDuModelUseCase : IObtenirArticlesDuModelUseCase
    {
        private readonly IModelDeListeGateway _gateway;
        public ObtenirArticlesDuModelUseCase(IModelDeListeGateway gateway) => _gateway = gateway;
        public async Task<IEnumerable<Article>> ExecuterAsync(int modelId) => await _gateway.ObtenirArticlesDuModelAsync(modelId);
    }
}