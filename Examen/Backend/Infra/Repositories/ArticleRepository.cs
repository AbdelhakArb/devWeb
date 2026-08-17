using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Infra.Repositories.Abstractions;
using Infra.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infra.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly string _connectionString;

        public ArticleRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ??
                throw new ArgumentNullException(nameof(configuration), "La chaîne de connexion MySQL est introuvable.");
        }

        private MySqlConnection GetConnection() => new MySqlConnection(_connectionString);
        
        public PresenceArticleDansListe? GetPresenceArticleById(int presenceArticleId)
        {
            using var connection = GetConnection();
            var sql = "SELECT * FROM presencearticledansliste WHERE idPresenceArticleDansListe = @Id";
            
            return connection.QuerySingleOrDefault<PresenceArticleDansListe>(sql, new { Id = presenceArticleId });
        }

        public void CreateReservation(Infra.Models.Reservation reservation)
        {
            using var connection = GetConnection();
            var sql = @"INSERT INTO reservation (quantiteReservee, nomVisiteur, messageVisiteur, statusReservation, presenceArticleDansListeId) 
                        VALUES (@QuantiteReservee, @NomVisiteur, @MessageVisiteur, @StatusReservation, @PresenceArticleDansListeId);";

            connection.Execute(sql, reservation);
        }

        public async Task<IEnumerable<Article>> ObtenirTousLesArticlesAsync()
        {
            using var connection = GetConnection();
            var sql = "SELECT * FROM article"; 
            
            return await connection.QueryAsync<Article>(sql);
        }

        public async Task<Article?> ObtenirArticleParIdAsync(int articleId)
        {
            using var connection = GetConnection();
            var sql = "SELECT * FROM article WHERE articleId = @ArticleId";
            return await connection.QuerySingleOrDefaultAsync<Article>(sql, new { ArticleId = articleId });
        }
    }
}