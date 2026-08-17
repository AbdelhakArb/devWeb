using System.Collections.Generic;
using System.Threading.Tasks;
using Infra.Repositories.Abstractions;
using ListeDeNaissance.Core.IGateways;
using ListeDeNaissance.Core.Models;

namespace Infra.Gateway
{
    public class ModelDeListeGateway : IModelDeListeGateway
    {
        private readonly IModelDeListeRepository _repository;

        public ModelDeListeGateway(IModelDeListeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ModelDeListeDeNaissance>> ObtenirTousLesModelsAsync()
        {
            return await _repository.ObtenirTousLesModelsAsync();
        }

        public async Task<IEnumerable<Article>> ObtenirArticlesDuModelAsync(int modelId)
        {
            return await _repository.ObtenirArticlesDuModelAsync(modelId);
        }
    }
}