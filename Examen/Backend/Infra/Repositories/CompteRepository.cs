using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Infra.Repositories.Abstractions;

namespace Infra.Repositories
{
    public class CompteRepository : ICompteRepository
    {
        private readonly string _connectionString;

        public CompteRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ??
                throw new ArgumentNullException(nameof(configuration), "La chaîne de connexion MySQL est introuvable.");
        }

        private MySqlConnection GetConnection() => new MySqlConnection(_connectionString);

        // On utilise la classe CompteParent exigée par l'interface
        public global::ListeDeNaissance.Core.Models.CompteParent? GetCompteByEmail(string email)
        {
            using var connection = GetConnection();
            var sql = "SELECT * FROM compte WHERE emailCompte = @Email";
            
            // Dapper va remplir directement un objet CompteParent avec les données de ta table SQL
            return connection.QuerySingleOrDefault<global::ListeDeNaissance.Core.Models.CompteParent>(sql, new { Email = email });
        }
    }
}