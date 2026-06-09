using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Infra.Repositories.Abstractions;
using CoreModels = ListeDeNaissance.Core.Models; // Notre alias fétiche pour le Core

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

        // On utilise le modèle PresenceArticleDansListe du Core
        public CoreModels.PresenceArticleDansListe? GetPresenceArticleById(int presenceArticleId)
        {
            using var connection = GetConnection();
            var sql = "SELECT * FROM presencearticledansliste WHERE idPresenceArticleDansListe = @Id";
            
            return connection.QuerySingleOrDefault<CoreModels.PresenceArticleDansListe>(sql, new { Id = presenceArticleId });
        }

        // On utilise le modèle Reservation du Core
        public void CreateReservation(CoreModels.Reservation reservation)
        {
            using var connection = GetConnection();
            var sql = @"INSERT INTO reservation (quantiteReservee, nomVisiteur, messageVisiteur, statusReservation, presenceArticleDansListeId) 
                        VALUES (@QuantiteReservee, @NomVisiteur, @MessageVisiteur, @StatusReservation, @PresenceArticleDansListeId);";

            connection.Execute(sql, reservation);
        }
    }
}