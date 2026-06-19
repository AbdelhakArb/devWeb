using System.Collections.Generic;
using System.Threading.Tasks;
using Infra.Repositories.Abstractions;
using MySql.Data.MySqlClient; 

namespace Infra.Repositories
{
    public class ListeDeNaissanceRepository : IListeDeNaissanceRepository
    {
        private readonly string _connectionString = "Server=localhost;Database=listedenaissance;Uid=root;Pwd=coucou";

        public async Task<IEnumerable<Models.ListeDeNaissance>> GetAllAsync()
        {
            // Code d'accès DB fictif (À adapter selon ton Dapper/EF/ADO)
            var listes = new List<Models.ListeDeNaissance>();
            return await Task.FromResult(listes);
        }

        public async Task<Models.ListeDeNaissance?> GetByIdAsync(int id)
        {
            return await Task.FromResult<Models.ListeDeNaissance?>(null);
        }

        public async Task<IEnumerable<Models.ListeDeNaissance>> GetByParentIdAsync(int parentId)
        {
            var listes = new List<Models.ListeDeNaissance>();
            return await Task.FromResult(listes);
        }

        public async Task InsertAsync(Models.ListeDeNaissance liste)
        {
            await Task.CompletedTask;
        }

        public async Task UpdateAsync(Models.ListeDeNaissance liste)
        {
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            await Task.CompletedTask;
        }
    }
}