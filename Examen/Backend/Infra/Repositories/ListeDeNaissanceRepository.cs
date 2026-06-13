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
                (item, art) =>
                {
                    item.Article = art;
                    return item;
                },
                new { ListeId = listeId },
                splitOn: "Id"
            );

            return result.ToList();
        }
        public async Task<int> IncrementerQuantiteArticleAsync(int listeId, int articleId)
        {
            using var connection = GetConnection();
            await connection.OpenAsync(); // Obligatoire pour utiliser les transactions
            using var transaction = await connection.BeginTransactionAsync();

            try
            {
                // 1. Décrémenter le stock de l'article dans le magasin
                var sqlUpdateStock = @"UPDATE article 
                               SET articleQty = articleQty - 1 
                               WHERE articleId = @ArticleId;";
                await connection.ExecuteAsync(sqlUpdateStock, new { ArticleId = articleId }, transaction);

                // 2. Incrémenter la quantité souhaitée dans la liste de naissance
                var sqlUpdatePivot = @"UPDATE presencearticledansliste 
                               SET qtySouhaitee = qtySouhaitee + 1 
                               WHERE listeDeNaissanceId = @ListeId AND articleId = @ArticleId;";
                await connection.ExecuteAsync(sqlUpdatePivot, new { ListeId = listeId, ArticleId = articleId }, transaction);

                // 3. Récupérer la nouvelle quantité souhaitée pour la renvoyer au front
                var sqlSelectNewQty = @"SELECT qtySouhaitee 
                                FROM presencearticledansliste 
                                WHERE listeDeNaissanceId = @ListeId AND articleId = @ArticleId;";
                int nouvelleQty = await connection.QueryFirstOrDefaultAsync<int>(sqlSelectNewQty, new { ListeId = listeId, ArticleId = articleId }, transaction);

                // Si tout s'est bien passé, on valide la transaction dans la base de données
                await transaction.CommitAsync();

                return nouvelleQty;
            }
            catch (Exception ex)
            {
                // En cas d'erreur (plus de stock, coupure réseau, etc.), on annule TOUT
                await transaction.RollbackAsync();
                Console.WriteLine($"Erreur lors de l'incrémentation : {ex.Message}");
                throw;
            }
        }
        public async Task<int> DecrementerQuantiteArticleAsync(int listeId, int articleId)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();

            try
            {
                // 1. Récupérer la quantité souhaitée actuelle pour cet article dans cette liste
                var sqlSelectQty = @"SELECT qtySouhaitee 
                             FROM presencearticledansliste 
                             WHERE listeDeNaissanceId = @ListeId AND articleId = @ArticleId;";

                int qtyActuelle = await connection.QueryFirstOrDefaultAsync<int>(sqlSelectQty, new { ListeId = listeId, ArticleId = articleId }, transaction);

                if (qtyActuelle <= 0)
                {
                    throw new Exception("L'article n'est pas présent ou a déjà une quantité à 0.");
                }

                // 2. Dans tous les cas, on redonne +1 au stock global du magasin
                var sqlUpdateStock = @"UPDATE article 
                               SET articleQty = articleQty + 1 
                               WHERE articleId = @ArticleId;";
                await connection.ExecuteAsync(sqlUpdateStock, new { ArticleId = articleId }, transaction);

                int nouvelleQty = 0;

                if (qtyActuelle == 1)
                {
                    // Cas A : La quantité tombe à 0 -> On supprime l'article de la liste
                    var sqlDeletePivot = @"DELETE FROM presencearticledansliste 
                                   WHERE listeDeNaissanceId = @ListeId AND articleId = @ArticleId;";
                    await connection.ExecuteAsync(sqlDeletePivot, new { ListeId = listeId, ArticleId = articleId }, transaction);

                    nouvelleQty = 0;
                }
                else
                {
                    // Cas B : La quantité est supérieure à 1 -> On fait -1
                    var sqlUpdatePivot = @"UPDATE presencearticledansliste 
                                   SET qtySouhaitee = qtySouhaitee - 1 
                                   WHERE listeDeNaissanceId = @ListeId AND articleId = @ArticleId;";
                    await connection.ExecuteAsync(sqlUpdatePivot, new { ListeId = listeId, ArticleId = articleId }, transaction);

                    nouvelleQty = qtyActuelle - 1;
                }

                // Si tout s'est bien passé, on valide la transaction en base
                await transaction.CommitAsync();

                return nouvelleQty;
            }
            catch (Exception ex)
            {
                // Une tuile ? On annule tout pour garder la BDD propre
                await transaction.RollbackAsync();
                Console.WriteLine($"Erreur lors de la décrémentation : {ex.Message}");
                throw;
            }
        }
        public async Task<bool> SoumettreReservationsAsync(CoreModels.PanierReservationDto panier)
        {
            if (panier.ListeReservations == null || panier.ListeReservations.Count == 0)
            {
                throw new Exception("Le panier de réservation est vide.");
            }

            using var connection = GetConnection();
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();

            try
            {
                foreach (var item in panier.ListeReservations)
                {
                    // 1. Récupérer la quantité souhaitée et la somme des quantités déjà réservées pour cette ligne pivot
                    var sqlVerif = @"
                SELECT 
                    p.qtySouhaitee AS QtySouhaitee,
                    COALESCE(SUM(r.qtyReserve), 0) AS QtyDejaReservee
                FROM presencearticledansliste p
                LEFT JOIN reservation r ON p.presenceArticleDansListeId = r.presenceArticleDansListeId
                WHERE p.presenceArticleDansListeId = @PresenceId
                GROUP BY p.qtySouhaitee;";

                    var info = await connection.QueryFirstOrDefaultAsync<dynamic>(sqlVerif, new { PresenceId = item.PresenceArticleListeId }, transaction);

                    if (info == null)
                    {
                        throw new Exception($"L'article de liste avec l'ID {item.PresenceArticleListeId} n'existe pas.");
                    }

                    int qtySouhaitee = info.QtySouhaitee;
                    int qtyDejaReservee = (int)info.QtyDejaReservee;
                    int qtyRestante = qtySouhaitee - qtyDejaReservee;

                    // 2. Vérification de la contrainte (comme dans ton code TS)
                    if (item.QtyReserve <= 0 || item.QtyReserve > qtyRestante)
                    {
                        throw new Exception($"Réservation impossible. Quantité restante insuffisante ({qtyRestante} disponible(s), demandé : {item.QtyReserve}).");
                    }

                    // 3. Création de la ligne de réservation
                    var sqlInsertReservation = @"
                INSERT INTO reservation (visiteurId, presenceArticleDansListeId, qtyReserve, dateReservation)
                VALUES (@VisiteurId, @PresenceId, @QtyReserve, @DateReservation);";

                    await connection.ExecuteAsync(sqlInsertReservation, new
                    {
                        VisiteurId = panier.VisiteurId,
                        PresenceId = item.PresenceArticleListeId,
                        QtyReserve = item.QtyReserve,
                        DateReservation = DateTime.Now
                    }, transaction);
                }

                // Si toutes les lignes du panier sont validées, on valide la transaction globale
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Au moindre problème ou dépassement de quantité, on annule TOUT pour éviter les doublons
                await transaction.RollbackAsync();
                Console.WriteLine($"[Transaction Annulée] Erreur réservation : {ex.Message}");
                throw;
            }
        }
    }
}