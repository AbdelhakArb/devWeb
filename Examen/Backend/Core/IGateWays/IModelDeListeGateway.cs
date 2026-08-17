using System.Collections.Generic;
using System.Threading.Tasks;
using ListeDeNaissance.Core.Models;

namespace ListeDeNaissance.Core.IGateways
{
    public interface IModelDeListeGateway
    {
        Task<IEnumerable<ListeDeNaissance.Core.Models.ModelDeListeDeNaissance>> ObtenirTousLesModelsAsync();
        Task<IEnumerable<ListeDeNaissance.Core.Models.Article>> ObtenirArticlesDuModelAsync(int modelId);
    }
}