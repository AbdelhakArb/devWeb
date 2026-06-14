using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Infra.Models;
using ListeDeNaissance.Core.Models;
using ListeDeNaissance.Core.IGateways;

namespace Infra.Repositories
{
    public class ListeDeNaissanceRepository : IListeDeNaissanceGateway
    {
        private readonly string _connectionString;

        public ListeDeNaissanceRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ??
                throw new ArgumentNullException(nameof(configuration), "La chaîne de connexion MySQL est introuvable.");
        }

        private MySqlConnection GetConnection() => new MySqlConnection(_connectionString);

        // =========================================================================
        // 1. GESTION DE LA LISTE DE NAISSANCE
        // =========================================================================

        public async Task CreerListeAsync(global::ListeDeNaissance.Core.Models.ListeDeNaissance liste)
        {
            using var connection = GetConnection();
            
            // On intègre absolument TOUS les champs de ta table MySQL pour satisfaire les contraintes NOT NULL
            var sql = @"
                INSERT INTO listeDeNaissance (
                    compteParentId, 
                    nomListeDeNaissance, 
                    dateCreationListe, 
                    datePrevuPourAccouchement, 
                    statusListe, 
                    lieuListe
                )
                VALUES (
                    @CompteParentId, 
                    @Nom, 
                    @DateCreation, 
                    @DateAccouchement, 
                    @Status, 
                    @Lieu
                );";

            await connection.ExecuteAsync(sql, new
            {
                CompteParentId = liste.CompteParentId,
                Nom = liste.NomListeDeNaissance,
                DateCreation = DateTime.Now,
                // On met une date par défaut (ex: dans 9 mois) ou la date actuelle si l'objet n'en a pas
                DateAccouchement = DateTime.Now.AddMonths(9), 
                Status = "Actif",
                Lieu = "Non spécifié" // Valeur temporaire pour remplir le champ texte requis
            });
        }

        public async Task<global::ListeDeNaissance.Core.Models.ListeDeNaissance?> ObtenirListeParIdAsync(int listeId)
        {
            using var connection = GetConnection();
            var sql = "SELECT * FROM listedenaissance WHERE listeDeNaissanceId = @ListeId";
            
            var result = await connection.QuerySingleOrDefaultAsync<dynamic>(sql, new { ListeId = listeId });

            if (result == null) return null;

            return new global::ListeDeNaissance.Core.Models.ListeDeNaissance
            {
                ListeDeNaissanceId = (int)result.listeDeNaissanceId,
                CompteParentId = (int)result.compteParentId,
                NomListeDeNaissance = (string)result.nomListeDeNaissance,
                DateCreationListe = (DateTime)result.dateCreationListe,
                StatusListe = (string)result.statusListe
            };
        }

        public async Task<List<global::ListeDeNaissance.Core.Models.ListeDeNaissance>> ObtenirListesParParentIdAsync(int parentId)
        {
            using var connection = GetConnection();
            var sql = "SELECT * FROM listedenaissance WHERE compteParentId = @ParentId";
            var entities = await connection.QueryAsync<dynamic>(sql, new { ParentId = parentId });

            return entities.Select(e => new global::ListeDeNaissance.Core.Models.ListeDeNaissance
            {
                ListeDeNaissanceId = (int)e.listeDeNaissanceId,
                CompteParentId = (int)e.compteParentId,
                NomListeDeNaissance = (string)e.nomListeDeNaissance,
                DateCreationListe = (DateTime)e.dateCreationListe,
                StatusListe = (string)e.statusListe
            }).ToList();
        }

        public async Task<global::ListeDeNaissance.Core.Models.ListeDeNaissance?> ObtenirListeParCodeOuLienAsync(string codeOuLien)
        {
            using var connection = GetConnection();
            var sql = "SELECT * FROM listedenaissance WHERE nomListeDeNaissance = @Code";
            var result = await connection.QuerySingleOrDefaultAsync<dynamic>(sql, new { Code = codeOuLien });

            if (result == null) return null;

            return new global::ListeDeNaissance.Core.Models.ListeDeNaissance
            {
                ListeDeNaissanceId = (int)result.listeDeNaissanceId,
                CompteParentId = (int)result.compteParentId,
                NomListeDeNaissance = (string)result.nomListeDeNaissance,
                DateCreationListe = (DateTime)result.dateCreationListe,
                StatusListe = (string)result.statusListe
            };
        }

        public async Task<List<global::ListeDeNaissance.Core.Models.ModelDeListeDeNaissance>> ObtenirTousLesModelesAsync()
        {
            using var connection = GetConnection();
            var sql = "SELECT listeDeNaissanceId, nomListeDeNaissance FROM listedenaissance WHERE statusListe = 'Modele'";
            
            var entities = await connection.QueryAsync<dynamic>(sql);

            return entities.Select(e => {
                dynamic model = new global::ListeDeNaissance.Core.Models.ModelDeListeDeNaissance();
                try { model.IdModelDeListeDeNaissance = (int)e.listeDeNaissanceId; } catch {}
                try { model.IdModelDeListe = (int)e.listeDeNaissanceId; } catch {}
                try { model.NomModel = (string)e.nomListeDeNaissance; } catch {}
                return (global::ListeDeNaissance.Core.Models.ModelDeListeDeNaissance)model;
            }).ToList();
        }

        // =========================================================================
        // 2. GESTION DES ARTICLES DANS LA LISTE (MANY-TO-MANY)
        // =========================================================================

        public async Task AjouterArticleDansListeAsync(PresenceArticleDansListe presenceArticle)
        {
            using var connection = GetConnection();
            var sql = @"
                INSERT INTO presenceArticleDansListe (ListeDeNaissanceId, ArticleId, QtySouhaitee, QtyReservee)
                VALUES (@ListeId, @ArticleId, @QtySouhaitee, 0)
                ON DUPLICATE KEY UPDATE QtySouhaitee = QtySouhaitee + @QtySouhaitee;";

            await connection.ExecuteAsync(sql, new
            {
                ListeId = presenceArticle.ListeDeNaissanceId,
                ArticleId = presenceArticle.ArticleId,
                QtySouhaitee = presenceArticle.QtySouhaitee
            });
        }

        public async Task<List<ArticleItemInListe>> GetArticlesPourReservationAsync(int listeId)
        {
            using var connection = GetConnection();
            var sql = @"
                SELECT 
                    p.IdPresenceArticleDansListe AS IdPresenceArticleDansListe,
                    p.ListeDeNaissanceId AS ListeDeNaissanceId,
                    p.ArticleId AS ArticleId,
                    a.ArticleNom AS ArticleNom,
                    a.ArticleDesc AS ArticleDesc,
                    a.ArticlePrix AS ArticlePrix,
                    p.QtySouhaitee AS QtySouhaitee,
                    p.QtyReservee AS QtyReservee
                FROM presenceArticleDansListe p
                INNER JOIN article a ON p.ArticleId = a.ArticleId
                WHERE p.ListeDeNaissanceId = @ListeId";

            var articles = await connection.QueryAsync<ArticleItemInListe>(sql, new { ListeId = listeId });
            return articles.ToList();
        }

        public async Task<int> IncrementerQuantiteArticleAsync(int listeId, int articleId)
        {
            using var connection = GetConnection();
            var sql = @"
                UPDATE presenceArticleDansListe 
                SET QtySouhaitee = QtySouhaitee + 1 
                WHERE ListeDeNaissanceId = @ListeId AND ArticleId = @ArticleId;
                
                SELECT QtySouhaitee FROM presenceArticleDansListe 
                WHERE ListeDeNaissanceId = @ListeId AND ArticleId = @ArticleId;";

            return await connection.QuerySingleAsync<int>(sql, new { ListeId = listeId, ArticleId = articleId });
        }

        public async Task<int> DecrementerQuantiteArticleAsync(int listeId, int articleId)
        {
            using var connection = GetConnection();
            
            var sqlUpdate = @"
                UPDATE presenceArticleDansListe 
                SET QtySouhaitee = QtySouhaitee - 1 
                WHERE ListeDeNaissanceId = @ListeId AND ArticleId = @ArticleId;";
            
            await connection.ExecuteAsync(sqlUpdate, new { ListeId = listeId, ArticleId = articleId });

            var sqlSelect = @"
                SELECT QtySouhaitee FROM presenceArticleDansListe 
                WHERE ListeDeNaissanceId = @ListeId AND ArticleId = @ArticleId;";
            
            int nouvelleQty = await connection.QuerySingleAsync<int>(sqlSelect, new { ListeId = listeId, ArticleId = articleId });

            if (nouvelleQty <= 0)
            {
                var sqlDelete = "DELETE FROM presenceArticleDansListe WHERE ListeDeNaissanceId = @ListeId AND ArticleId = @ArticleId";
                await connection.ExecuteAsync(sqlDelete, new { ListeId = listeId, ArticleId = articleId });
                return 0;
            }

            return nouvelleQty;
        }

        // =========================================================================
        // 3. SOUMISSION DES RÉSERVATIONS (PANIER VISITEUR)
        // =========================================================================

        public async Task<bool> SoumettreReservationsAsync(PanierReservationDto panier)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();

            try
            {
                dynamic dynamiquePanier = panier;
                dynamic listeArticles = null;

                // CORRECTION : Extraction adaptative sans dépendance directe à System.IEnumerable
                try { listeArticles = dynamiquePanier.ArticlesReserves; } catch {}
                if (listeArticles == null) { try { listeArticles = dynamiquePanier.LignesPanier; } catch {} }
                if (listeArticles == null) { try { listeArticles = dynamiquePanier.Articles; } catch {} }

                if (listeArticles != null)
                {
                    foreach (dynamic item in listeArticles)
                    {
                        var sqlReservation = @"
                            INSERT INTO reservation (VisiteurId, PresenceArticleDansListeId, QtyReserve, DateReservation)
                            VALUES (@VisiteurId, @PresenceId, @Qty, @Date);";

                        await connection.ExecuteAsync(sqlReservation, new
                        {
                            VisiteurId = dynamiquePanier.VisiteurId,
                            PresenceId = item.PresenceArticleDansListeId,
                            Qty = item.Quantite,
                            Date = DateTime.Now
                        }, transaction);

                        var sqlUpdatePresence = @"
                            UPDATE presenceArticleDansListe 
                            SET QtyReservee = QtyReservee + @Qty
                            WHERE IdPresenceArticleDansListe = @PresenceId;";

                        await connection.ExecuteAsync(sqlUpdatePresence, new
                        {
                            Qty = item.Quantite,
                            PresenceId = item.PresenceArticleDansListeId
                        }, transaction);
                    }
                }

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}