using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using ListeDeNaissance.Core.IGateways;
using CoreModels = ListeDeNaissance.Core.Models; // Alias propre pour les modèles du Core

namespace Infra.Repositories
{
    public class ListeDeNaissanceRepository : IListeDeNaissanceGateway
    {
        private readonly string _connectionString;

        public ListeDeNaissanceRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ??
                throw new ArgumentNullException(nameof(configuration), "La chaîne de connexion MySQL 'DefaultConnection' est introuvable.");
        }

        private MySqlConnection GetConnection() => new MySqlConnection(_connectionString);

        // 1. Créer une liste (CORRIGÉ : Ajout de la colonne manquante datePrevuPourAccouchement)
        public async Task CreerListeAsync(CoreModels.ListeDeNaissance liste)
        {
            using var connection = GetConnection();
            var sql = @"INSERT INTO listedenaissance (nomListeDeNaissance, dateCreationListe, statusListe, compteParentId, datePrevuPourAccouchement) 
                        VALUES (@NomListeDeNaissance, @DateCreationListe, @StatusListe, @CompteParentId, @DatePrevuPourAccouchement);";

            // Dapper va lire les propriétés de l'objet du Core pour exécuter le SQL
            await connection.ExecuteAsync(sql, liste);
        }

        // 2. Obtenir une liste par son Identifiant unique
        public async Task<CoreModels.ListeDeNaissance?> ObtenirListeParIdAsync(int listeId)
        {
            using var connection = GetConnection();
            var sql = "SELECT * FROM listedenaissance WHERE idListeDeNaissance = @Id";
            
            // On demande à Dapper de mapper directement le résultat SQL dans le modèle du Core
            return await connection.QuerySingleOrDefaultAsync<CoreModels.ListeDeNaissance>(sql, new { Id = listeId });
        }

        // 3. Obtenir toutes les listes d'un parent
        public async Task<List<CoreModels.ListeDeNaissance>> ObtenirListesParParentIdAsync(int compteParentId)
        {
            using var connection = GetConnection();
            var sql = "SELECT * FROM listedenaissance WHERE compteParentId = @ParentId";
            
            var result = await connection.QueryAsync<CoreModels.ListeDeNaissance>(sql, new { ParentId = compteParentId });
            return result.AsList();
        }

        // 4. Obtenir une liste via son code de partage ou lien unique
        public async Task<CoreModels.ListeDeNaissance?> ObtenirListeParCodeOuLienAsync(string code)
        {
            using var connection = GetConnection();
            var sql = "SELECT * FROM listedenaissance WHERE codeUniqueListe = @Code"; 
            
            return await connection.QuerySingleOrDefaultAsync<CoreModels.ListeDeNaissance>(sql, new { Code = code });
        }

        // 5. Obtenir tous les modèles de listes existants
        public async Task<List<CoreModels.ModelDeListeDeNaissance>> ObtenirTousLesModelesAsync()
        {
            using var connection = GetConnection();
            var sql = "SELECT * FROM modeldelistedenaissance"; 
            
            var result = await connection.QueryAsync<CoreModels.ModelDeListeDeNaissance>(sql);
            return result.AsList();
        }
    }
}