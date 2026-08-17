using System.Threading.Tasks;
using ListeDeNaissance.Core.Models;

namespace ListeDeNaissance.Core.UseCases.Abstractions
{
    public interface IAfficherListesParentParParentIdUseCase
    {
        Task<IEnumerable<ListeDeNaissance.Core.Models.ListeDeNaissance>> ExecuterAsync(int parentId);
    }
}