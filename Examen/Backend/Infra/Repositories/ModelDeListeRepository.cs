using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Infra.Repositories.Abstractions;
using ListeDeNaissance.Core.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Infra.Repositories
{
    public class ModelDeListeRepository : IModelDeListeRepository
    {
        private readonly string _connectionString;

        public ModelDeListeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ??
                throw new ArgumentNullException(nameof(configuration), "La chaîne de connexion MySQL est introuvable.");
        }

        private IDbConnection Connection => new MySqlConnection(_connectionString);

        public async Task<IEnumerable<ModelDeListeDeNaissance>> ObtenirTousLesModelsAsync()
        {
            using (var db = Connection)
            {
                string sql = @"SELECT 
                                modellisteId AS ModellisteId, 
                                modellisteNom AS ModellisteNom, 
                                modellisteDesc AS ModellisteDesc 
                               FROM modeldelistedenaissance";

                return await db.QueryAsync<ModelDeListeDeNaissance>(sql);
            }
        }

        public async Task<IEnumerable<Article>> ObtenirArticlesDuModelAsync(int modelId)
        {
            using (var db = Connection)
            {
                string sql = @"SELECT 
                                a.articleId AS ArticleId, 
                                a.articleNom AS ArticleNom, 
                                a.articleDesc AS ArticleDesc, 
                                a.articlePrix AS ArticlePrix, 
                                1 AS ArticleQty 
                               FROM article a 
                               JOIN presencearticledansmodel p ON a.articleId = p.articleId 
                               WHERE p.modellisteId = @ModelId";

                return await db.QueryAsync<Article>(sql, new { ModelId = modelId });
            }
        }
    }
}