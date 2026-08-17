using System.Collections.Generic;
using System.Threading.Tasks;
using ListeDeNaissance.Core.Models;

namespace Infra.Repositories.Abstractions
{
    public interface IModelDeListeRepository
    {
        Task<IEnumerable<ListeDeNaissance.Core.Models.ModelDeListeDeNaissance>> ObtenirTousLesModelsAsync();
        Task<IEnumerable<ListeDeNaissance.Core.Models.Article>> ObtenirArticlesDuModelAsync(int modelId);
    }
}