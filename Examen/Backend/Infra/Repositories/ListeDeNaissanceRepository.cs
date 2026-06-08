using Dapper;
using Infra.Models;
using Infra.Repositories.Abstractions;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Infra.Repositories
{
    public class ListeDeNaissanceRepository : IListeDeNaissanceRepository
    {
        private readonly string _connectionString;

        // Le constructeur récupère la chaîne de connexion de la base de données, comme chez ton prof
        public ListeDeNaissanceRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ??
                throw new ArgumentNullException(nameof(configuration), "Database connection string 'DefaultConnection' not found.");
        }

        // Cette méthode permet d'ouvrir proprement une connexion à MySQL
        private MySqlConnection GetConnection() => new MySqlConnection(_connectionString);

        // 1. Récupérer une liste par son Identifiant unique
        public ListeDeNaissance? GetListeById(int listeId)
        {
            using var connection = GetConnection();
            var sql = "SELECT * FROM listedenaissance WHERE idListeDeNaissance = @Id";
            return connection.QuerySingleOrDefault<ListeDeNaissance>(sql, new { Id = listeId });
        }

        // 2. Récupérer toutes les listes d'un parent spécifique (Règle : un parent peut en avoir plusieurs !)
        public IEnumerable<ListeDeNaissance> GetListesByParentId(int compteParentId)
        {
            using var connection = GetConnection();
            var sql = "SELECT * FROM listedenaissance WHERE compteParentId = @ParentId";
            return connection.Query<ListeDeNaissance>(sql, new { ParentId = compteParentId });
        }

        // 3. Insérer (Créer) une nouvelle liste de naissance dans MySQL
        public void CreateListe(ListeDeNaissance liste)
        {
            using var connection = GetConnection();
            var sql = @"INSERT INTO listedenaissance (nomListeDeNaissance, dateCreationListe, statusListe, compteParentId) 
                        VALUES (@NomListeDeNaissance, @DateCreationListe, @StatusListe, @CompteParentId);";

            connection.Execute(sql, liste);
        }
    }
}