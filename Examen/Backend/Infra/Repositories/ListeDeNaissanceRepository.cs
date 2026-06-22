using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Infra.Repositories.Abstractions;
using MySql.Data.MySqlClient; 
using Dapper; 
using ListeDeNaissance.Core.Models;

namespace Infra.Repositories
{
    public class ListeDeNaissanceRepository : IListeDeNaissanceRepository
    {
        private readonly string _connectionString = "Server=localhost;Database=listedenaissance;Uid=root;Pwd=coucou";

        // Propriété utilitaire pour créer et ouvrir une connexion rapidement
        private IDbConnection Connection => new MySqlConnection(_connectionString);
        
        public async Task<IEnumerable<Models.ListeDeNaissance>> GetAllAsync()
        {
            using (var db = Connection)
            {
                string sql = "SELECT IdListeDeNaissance, CompteParentId, NomListeDeNaissance FROM listedenaissance";
                // Dapper mappe automatiquement le résultat de la requête SQL dans ton modèle d'Infra
                return await db.QueryAsync<Models.ListeDeNaissance>(sql);
            }
        }


        public async Task<Models.ListeDeNaissance?> GetByIdAsync(int id)
        {
            using (var db = Connection)
            {
                string sql = "SELECT IdListeDeNaissance, CompteParentId, NomListeDeNaissance FROM listedenaissance WHERE IdListeDeNaissance = @Id";
                return await db.QueryFirstOrDefaultAsync<Models.ListeDeNaissance>(sql, new { Id = id });
            }
        }

        public async Task<IEnumerable<Models.ListeDeNaissance>> GetByParentIdAsync(int parentId)
        {
            using (var db = Connection)
            {
                string sql = "SELECT IdListeDeNaissance, CompteParentId, NomListeDeNaissance FROM listedenaissance WHERE CompteParentId = @ParentId";
                return await db.QueryAsync<Models.ListeDeNaissance>(sql, new { ParentId = parentId });
            }
        }


        public async Task InsertAsync(Models.ListeDeNaissance liste)
        {
            using (var db = Connection)
            {
                string sql = "INSERT INTO listedenaissance (CompteParentId, NomListeDeNaissance, StatusListe) VALUES (@CompteParentId, @NomListeDeNaissance, @StatusListe)";
                await db.ExecuteAsync(sql, new 
                { 
                    liste.CompteParentId, 
                    liste.NomListeDeNaissance, 
                    StatusListe = "Actif" 
                });
            }
        }


        public async Task UpdateAsync(Models.ListeDeNaissance liste)
        {
            using (var db = Connection)
            {
                string sql = "UPDATE listedenaissance SET NomListeDeNaissance = @NomListeDeNaissance WHERE IdListeDeNaissance = @IdListeDeNaissance";
                await db.ExecuteAsync(sql, liste); // Passe directement l'objet, Dapper extrait les propriétés automatiquement
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var db = Connection)
            {
                string sql = "DELETE FROM listedenaissance WHERE IdListeDeNaissance = @Id";
                await db.ExecuteAsync(sql, new { Id = id });
            }
        }

        public async Task AjouterArticleDansListeAsync(int listeId, int articleId, int quantite)
        {
            using (var db = Connection)
            {
                string sql = "INSERT INTO presencearticledansliste (ListeDeNaissanceId, ArticleId, QtySouhaitee) VALUES (@ListeId, @ArticleId, @Quantite)";
                await db.ExecuteAsync(sql, new { ListeId = listeId, ArticleId = articleId, Quantite = quantite });
            }
        }


        public async Task<IEnumerable<Models.PresenceArticleDansListe>> GetArticlesPourReservationRepoAsync(int listeId)
        {
            using (var db = Connection)
            {
                string sql = "SELECT ListeDeNaissanceId, ArticleId, QtySouhaitee FROM presencearticledansliste WHERE ListeDeNaissanceId = @ListeId";
                return await db.QueryAsync<Models.PresenceArticleDansListe>(sql, new { ListeId = listeId });
            }
        }

        public async Task<int> IncrementerQuantiteArticleRepoAsync(int listeId, int articleId)
        {
            using (var db = Connection)
            {
                string sql = @"UPDATE presencearticledansliste SET QtySouhaitee = QtySouhaitee + 1 WHERE ListeDeNaissanceId = @ListeId AND ArticleId = @ArticleId;
                               SELECT QtySouhaitee FROM presencearticledansliste WHERE ListeDeNaissanceId = @ListeId AND ArticleId = @ArticleId;";
                
                // ExecuteScalar renvoie la première colonne de la première ligne (le SELECT juste après l'UPDATE)
                return await db.ExecuteScalarAsync<int>(sql, new { ListeId = listeId, ArticleId = articleId });
            }
        }

        public async Task<int> DecrementerQuantiteArticleRepoAsync(int listeId, int articleId)
        {
            using (var db = Connection)
            {
                string selectSql = "SELECT QtySouhaitee FROM presencearticledansliste WHERE ListeDeNaissanceId = @ListeId AND ArticleId = @ArticleId";
                int quantiteActuelle = await db.ExecuteScalarAsync<int>(selectSql, new { ListeId = listeId, ArticleId = articleId });

                if (quantiteActuelle <= 1)
                {
                    string deleteSql = "DELETE FROM presencearticledansliste WHERE ListeDeNaissanceId = @ListeId AND ArticleId = @ArticleId";
                    await db.ExecuteAsync(deleteSql, new { ListeId = listeId, ArticleId = articleId });
                    return 0;
                }
                else
                {
                    string updateSql = @"UPDATE presencearticledansliste SET QtySouhaitee = QtySouhaitee - 1 WHERE ListeDeNaissanceId = @ListeId AND ArticleId = @ArticleId;
                                         SELECT QtySouhaitee FROM presencearticledansliste WHERE ListeDeNaissanceId = @ListeId AND ArticleId = @ArticleId;";
                    return await db.ExecuteScalarAsync<int>(updateSql, new { ListeId = listeId, ArticleId = articleId });
                }
            }
        }

       public async Task<bool> SoumettreReservationsRepoAsync(IEnumerable<Models.PresenceArticleDansListe> panier)
{
    using (var db = Connection)
    {
        db.Open();
        // On utilise une transaction car si un seul article du panier plante, on annule tout
        using (var transaction = db.BeginTransaction())
        {
            try
            {

                string sqlUpdate = @"UPDATE presencearticledansliste 
                                     SET QtySouhaitee = QtySouhaitee - @QtySouhaitee 
                                     WHERE ListeDeNaissanceId = @ListeId AND ArticleId = @ArticleId";

                foreach (var item in panier)
                {
                    await db.ExecuteAsync(sqlUpdate, new 
                    { 
                        QtySouhaitee = item.QtySouhaitee, 
                        ListeId = item.ListeDeNaissanceId, 
                        ArticleId = item.ArticleId 
                    }, transaction);
                }

                transaction.Commit();
                return true;
            }
            catch (System.Exception)
            {
                transaction.Rollback();
                return false;
            }
        }
    }
}
    }
}