using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infra.Repositories.Abstractions
{
    public interface IListeDeNaissanceRepository
    {
        Task<IEnumerable<Models.ListeDeNaissance>> GetAllAsync();
        Task<Models.ListeDeNaissance?> GetByIdAsync(int id);
        Task<IEnumerable<Models.ListeDeNaissance>> GetByParentIdAsync(int parentId);
        Task InsertAsync(Models.ListeDeNaissance liste);
        Task UpdateAsync(Models.ListeDeNaissance liste);
        Task DeleteAsync(int id);
    }
}