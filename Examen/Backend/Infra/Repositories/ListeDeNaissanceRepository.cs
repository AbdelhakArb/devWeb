using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Infra.Repositories.Abstractions;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Dapper;
using ListeDeNaissance.Core.Models;

namespace Infra.Repositories
{
    public class ListeDeNaissanceRepository : IListeDeNaissanceRepository
    {
        private readonly string _connectionString;

        public ListeDeNaissanceRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ??
                throw new ArgumentNullException(nameof(configuration), "La chaîne de connexion MySQL est introuvable.");
        }

        private IDbConnection Connection => new MySqlConnection(_connectionString);

        public async Task<IEnumerable<ListeDeNaissance.Core.Models.ListeDeNaissance>> GetAllAsync()
        {
            using (var db = Connection)
            {
                string sql = @"SELECT 
                                listeDeNaissanceId AS ListeDeNaissanceId, 
                                compteParentId AS CompteParentId, 
                                nomListeDeNaissance AS NomListeDeNaissance, 
                                dateCreationListe AS DateCreationListe, 
                                datePrevuPourAccouchement AS DatePrevuPourAccouchement,
                                statusListe AS StatusListe,
                                lieuListe AS LieuListe
                               FROM listedenaissance";

                return await db.QueryAsync<ListeDeNaissance.Core.Models.ListeDeNaissance>(sql);
            }
        }

        public async Task<ListeDeNaissance.Core.Models.ListeDeNaissance?> GetByIdAsync(int id)
        {
            using (var db = Connection)
            {
                string sql = @"SELECT 
                                listeDeNaissanceId AS ListeDeNaissanceId, 
                                compteParentId AS CompteParentId, 
                                nomListeDeNaissance AS NomListeDeNaissance, 
                                dateCreationListe AS DateCreationListe, 
                                datePrevuPourAccouchement AS DatePrevuPourAccouchement,
                                statusListe AS StatusListe,
                                lieuListe AS LieuListe
                               FROM listedenaissance 
                               WHERE listeDeNaissanceId = @Id";

                return await db.QueryFirstOrDefaultAsync<ListeDeNaissance.Core.Models.ListeDeNaissance>(sql, new { Id = id });
            }
        }

        public async Task<IEnumerable<ListeDeNaissance.Core.Models.ListeDeNaissance>> GetByParentIdAsync(int parentId)
        {
            using (var db = Connection)
            {
                string sql = @"SELECT 
                                listeDeNaissanceId AS ListeDeNaissanceId, 
                                compteParentId AS CompteParentId, 
                                nomListeDeNaissance AS NomListeDeNaissance, 
                                dateCreationListe AS DateCreationListe, 
                                datePrevuPourAccouchement AS DatePrevuPourAccouchement,
                                statusListe AS StatusListe,
                                lieuListe AS LieuListe
                               FROM listedenaissance 
                               WHERE compteParentId = @ParentId";

                return await db.QueryAsync<ListeDeNaissance.Core.Models.ListeDeNaissance>(sql, new { ParentId = parentId });
            }
        }

        public async Task InsertAsync(ListeDeNaissance.Core.Models.ListeDeNaissance liste)
        {
            using (var db = Connection)
            {
                string sql = @"INSERT INTO listedenaissance 
                               (compteParentId, nomListeDeNaissance, statusListe, dateCreationListe, datePrevuPourAccouchement, lieuListe) 
                               VALUES 
                               (@CompteParentId, @NomListeDeNaissance, @StatusListe, @DateCreation, @DatePrevuPourAccouchement, @LieuListe);
                               SELECT LAST_INSERT_ID();";

                var insertedId = await db.ExecuteScalarAsync<int>(sql, new
                {
                    liste.CompteParentId,
                    liste.NomListeDeNaissance,
                    StatusListe = string.IsNullOrEmpty(liste.StatusListe) ? "Actif" : liste.StatusListe,
                    DateCreation = DateTime.Now,
                    liste.DatePrevuPourAccouchement,
                    liste.LieuListe
                });

                liste.ListeDeNaissanceId = insertedId;
            }
        }

        public async Task UpdateAsync(ListeDeNaissance.Core.Models.ListeDeNaissance liste)
        {
            using (var db = Connection)
            {
                string sql = @"UPDATE listedenaissance 
                               SET nomListeDeNaissance = @NomListeDeNaissance, 
                                   datePrevuPourAccouchement = @DatePrevuPourAccouchement, 
                                   lieuListe = @LieuListe 
                               WHERE listeDeNaissanceId = @ListeDeNaissanceId";
                await db.ExecuteAsync(sql, liste);
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var db = Connection)
            {
                string sql = "DELETE FROM listedenaissance WHERE listeDeNaissanceId = @Id";
                await db.ExecuteAsync(sql, new { Id = id });
            }
        }

        public async Task AjouterArticleDansListeAsync(int listeId, int articleId, int quantite)
        {
            using (var db = Connection)
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        string sqlPresence = @"INSERT INTO presencearticledansliste (ListeDeNaissanceId, ArticleId, QtySouhaitee) 
                                             VALUES (@ListeId, @ArticleId, @Quantite)
                                             ON DUPLICATE KEY UPDATE QtySouhaitee = QtySouhaitee + @Quantite";

                        await db.ExecuteAsync(sqlPresence, new { ListeId = listeId, ArticleId = articleId, Quantite = quantite }, transaction);

                        string sqlArticle = @"UPDATE article SET articleQty = GREATEST(0, articleQty - @Quantite) WHERE articleId = @ArticleId";
                        await db.ExecuteAsync(sqlArticle, new { ArticleId = articleId, Quantite = quantite }, transaction);

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task MettreAJourQuantiteArticleAsync(int listeId, int articleId, int nouvelleQuantite)
        {
            using (var db = Connection)
            {
                string sql = @"UPDATE presencearticledansliste SET QtySouhaitee = @NouvelleQuantite WHERE ListeDeNaissanceId = @ListeId AND ArticleId = @ArticleId";
                await db.ExecuteAsync(sql, new { ListeId = listeId, ArticleId = articleId, NouvelleQuantite = nouvelleQuantite });
            }
        }

        public async Task<IEnumerable<PresenceArticleDansListe>> GetArticlesPourReservationRepoAsync(int listeId)
        {
            using (var db = Connection)
            {
                string sql = @"SELECT listeDeNaissanceId AS ListeDeNaissanceId, articleId AS ArticleId, qtySouhaitee AS QtySouhaitee, qtyReserve AS QtyReserve FROM presencearticledansliste WHERE listeDeNaissanceId = @ListeId";
                return await db.QueryAsync<PresenceArticleDansListe>(sql, new { ListeId = listeId });
            }
        }

        public async Task<IEnumerable<Article>> ObtenirArticlesParListeIdAsync(int listeId)
        {
            using (var db = Connection)
            {
                string sql = @"SELECT a.articleId AS ArticleId, a.articleNom AS ArticleNom, a.articleDesc AS ArticleDesc, 
                                       a.articlePrix AS ArticlePrix, p.qtySouhaitee AS ArticleQty,
                                       COALESCE((SELECT SUM(r.qtyReserve) FROM reservation r 
                                                 WHERE r.presenceArticleDansListeId = p.presenceArticleDansListeId), 0) AS QtyReserve
                               FROM article a 
                               JOIN presencearticledansliste p ON a.articleId = p.articleId 
                               WHERE p.listeDeNaissanceId = @ListeId";
                return await db.QueryAsync<Article>(sql, new { ListeId = listeId });
            }
        }

        public async Task<IEnumerable<Article>> ObtenirCatalogueArticlesAsync()
        {
            using (var db = Connection)
            {
                string sql = @"SELECT articleId AS ArticleId, articleNom AS ArticleNom, articleDesc AS ArticleDesc, articlePrix AS ArticlePrix, articleQty AS ArticleQty FROM article";
                return await db.QueryAsync<Article>(sql);
            }
        }

        public async Task<int> IncrementerQuantiteArticleRepoAsync(int listeId, int articleId)
        {
            using (var db = Connection)
            {
                string sql = @"UPDATE presencearticledansliste SET qtySouhaitee = qtySouhaitee + 1 WHERE listeDeNaissanceId = @ListeId AND articleId = @ArticleId";
                return await db.ExecuteAsync(sql, new { ListeId = listeId, ArticleId = articleId });
            }
        }

        public async Task<int> DecrementerQuantiteArticleRepoAsync(int listeId, int articleId)
        {
            using (var db = Connection)
            {
                string sql = @"UPDATE presencearticledansliste SET qtySouhaitee = GREATEST(0, qtySouhaitee - 1) WHERE listeDeNaissanceId = @ListeId AND articleId = @ArticleId";
                return await db.ExecuteAsync(sql, new { ListeId = listeId, ArticleId = articleId });
            }
        }

        public async Task<bool> SoumettreReservationsRepoAsync(IEnumerable<ReservationRequestItem> panier)
        {
            Console.WriteLine("[SoumettreReservationsRepoAsync] repo lance, panier count = " + panier.Count());

            var panierList = panier.ToList();
            if (!panierList.Any())
            {
                Console.WriteLine("[SoumettreReservationsRepoAsync] panier vide -> return false");
                return false;
            }

            foreach (var it in panierList)
            {
                Console.WriteLine($"[SoumettreReservationsRepoAsync] item reçu -> ListeDeNaissanceId={it.ListeDeNaissanceId}, ArticleId={it.ArticleId}, QtySouhaitee={it.QtySouhaitee}, VisiteurId={it.VisiteurId}");
            }

            using (var db = Connection)
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in panierList)
                        {
                            string sqlPresence = @"SELECT presenceArticleDansListeId, qtySouhaitee 
                                                   FROM presencearticledansliste 
                                                   WHERE listeDeNaissanceId = @ListeId AND articleId = @ArticleId 
                                                   FOR UPDATE";
                            var presence = await db.QueryFirstOrDefaultAsync(sqlPresence,
                                new { ListeId = item.ListeDeNaissanceId, ArticleId = item.ArticleId }, transaction);

                            if (presence == null)
                            {
                                Console.WriteLine($"[SoumettreReservationsRepoAsync] Article {item.ArticleId} introuvable dans la liste {item.ListeDeNaissanceId}");
                                transaction.Rollback();
                                return false;
                            }

                            int presenceId = (int)presence.presenceArticleDansListeId;
                            int qtySouhaitee = (int)presence.qtySouhaitee;

                            string sqlDejaReserve = @"SELECT COALESCE(SUM(qtyReserve), 0) 
                                                      FROM reservation 
                                                      WHERE presenceArticleDansListeId = @PresenceId";
                            int dejaReserve = await db.ExecuteScalarAsync<int>(sqlDejaReserve, new { PresenceId = presenceId }, transaction);

                            if (dejaReserve + item.QtySouhaitee > qtySouhaitee)
                            {
                                Console.WriteLine($"[SoumettreReservationsRepoAsync] Stock insuffisant pour l'article {item.ArticleId} (déjà réservé: {dejaReserve}, souhaité: {item.QtySouhaitee}, dispo: {qtySouhaitee})");
                                transaction.Rollback();
                                return false;
                            }

                            string sqlReservation = @"INSERT INTO reservation (visiteurId, qtyReserve, dateReservation, presenceArticleDansListeId) 
                                                      VALUES (@VisiteurId, @QtySouhaitee, NOW(), @PresenceId)";
                            await db.ExecuteAsync(sqlReservation, new
                            {
                                item.VisiteurId,
                                item.QtySouhaitee,
                                PresenceId = presenceId
                            }, transaction);
                        }

                        var premier = panierList.First();
                        if (!string.IsNullOrWhiteSpace(premier.MessageText))
                        {
                            string sqlMessage = @"INSERT INTO messages (listeDeNaissanceId, visiteurId, messageText, signatureMessage, dateMessage) 
                      VALUES (@ListeId, @VisiteurId, @MessageText, @SignatureMessage, NOW())";
                            await db.ExecuteAsync(sqlMessage, new
                            {
                                ListeId = premier.ListeDeNaissanceId,
                                premier.VisiteurId,
                                premier.MessageText,
                                premier.SignatureMessage
                            }, transaction);
                        }

                        transaction.Commit();
                        Console.WriteLine("[SoumettreReservationsRepoAsync] succès, commit effectué");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"[SoumettreReservationsRepoAsync] Erreur : {ex}");
                        return false;
                    }
                }
            }
        }

        public async Task EnregistrerConsultationRepoAsync(int listeId, int visiteurId)
        {
            using (var db = Connection)
            {
                string sql = @"INSERT INTO consultation (listeDeNaissanceId, visiteurId, dateConsultation) 
                               VALUES (@ListeId, @VisiteurId, NOW())
                               ON DUPLICATE KEY UPDATE dateConsultation = NOW()";
                await db.ExecuteAsync(sql, new { ListeId = listeId, VisiteurId = visiteurId });
            }
        }

        public async Task CloturerListeRepoAsync(int listeId, string statusListe)
        {
            using (var db = Connection)
            {
                string sql = @"UPDATE listedenaissance SET statusListe = @StatusListe WHERE listeDeNaissanceId = @ListeId";
                await db.ExecuteAsync(sql, new { ListeId = listeId, StatusListe = statusListe });
            }
        }
    }
}