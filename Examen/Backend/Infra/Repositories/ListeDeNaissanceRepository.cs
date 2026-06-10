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

        // 6. Ajouter un article dans une liste de naissance
public async Task AjouterArticleDansListeAsync(CoreModels.PresenceArticleDansListe presenceArticle)
{
    using var connection = GetConnection();
    
    // Le SQL vise la table pivot. On passe les ID et la quantité souhaitée.
    var sql = @"INSERT INTO presencearticledansliste (listeDeNaissanceId, articleId, qtySouhaitee) 
                VALUES (@ListeDeNaissanceId, @ArticleId, @QtySouhaitee);";

    await connection.ExecuteAsync(sql, new 
    {
        presenceArticle.ListeDeNaissanceId,
        presenceArticle.ArticleId,
        presenceArticle.QtySouhaitee
    });
}
public async Task<List<CoreModels.ArticleItemInListe>> GetArticlesPourReservationAsync(int listeId)
{
    using var connection = GetConnection();
    
    var sql = @"
        SELECT 
            p.presenceArticleDansListeId AS PresenceArticleListeId,
            p.listeDeNaissanceId AS ListeDeNaissanceId,
            p.qtySouhaitee AS QtySouhaitee,
            COALESCE(SUM(r.qtyReserve), 0) AS QtyReservee,
            (p.qtySouhaitee - COALESCE(SUM(r.qtyReserve), 0)) AS QtyRestante,
            a.ArticleId AS Id,
            a.ArticleNom AS Nom,
            a.ArticlePrix AS Prix
        FROM presencearticledansliste p
        INNER JOIN article a ON p.articleId = a.ArticleId
        LEFT JOIN reservation r ON p.presenceArticleDansListeId = r.presenceArticleDansListeId
        WHERE p.listeDeNaissanceId = @ListeId
        GROUP BY p.presenceArticleDansListeId, p.listeDeNaissanceId, p.qtySouhaitee, a.ArticleId, a.ArticleNom, a.ArticlePrix;";

    var result = await connection.QueryAsync<CoreModels.ArticleItemInListe, CoreModels.ArticleDto, CoreModels.ArticleItemInListe>(
        sql,
        (item, art) => {
            item.Article = art;
            return item;
        },
        new { ListeId = listeId },
        splitOn: "Id"
    );

    return result.ToList();
}
    }
}