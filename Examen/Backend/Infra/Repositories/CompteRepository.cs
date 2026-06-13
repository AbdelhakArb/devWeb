using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Infra.Repositories.Abstractions;
using Infra.Models; 
using ListeDeNaissance.Core.Models;
using BCrypt.Net;

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

        // =========================================================================
        // 1. LOGIQUE PARENT
        // =========================================================================

        public CompteParent? GetCompteByEmail(string email)
        {
            using var connection = GetConnection();
            var sql = "SELECT * FROM compteParent WHERE EmailDeContact = @Email";
            
            var entity = connection.QuerySingleOrDefault<CompteParentEntity>(sql, new { Email = email });

            if (entity == null) return null;

            return new CompteParent
            {
                CompteParentId = entity.CompteParentId,
                EmailDeContact = entity.EmailDeContact,
                MotDePasseCompte = entity.MotDePasseCompte,
                NomPremierParent = entity.NomPremierParent,
                PrenomPremierParent = entity.PrenomPremierParent
            };
        }

        public void CreateCompte(CompteParent parent)
        {
            using var connection = GetConnection();

            string passwordHache = BCrypt.Net.BCrypt.HashPassword(parent.MotDePasseCompte);

            var sql = @"
                INSERT INTO compteParent (EmailDeContact, MotDePasseCompte, NomPremierParent, PrenomPremierParent)
                VALUES (@Email, @Password, @Nom, @Prenom);";

            connection.Execute(sql, new
            {
                Email = parent.EmailDeContact,
                Password = passwordHache,   
                Nom = parent.NomPremierParent,     
                Prenom = parent.PrenomPremierParent 
            });
        }

        // =========================================================================
        // 2. LOGIQUE VISITEUR (CORRIGÉE)
        // =========================================================================

        public Visiteur? GetVisiteurByEmail(string email)
        {
            using var connection = GetConnection();
            // Assure-toi que ta table MySQL s'appelle bien 'visiteur'
            var sql = "SELECT * FROM visiteur WHERE VisiteurEmail = @Email";
            
            // On force Dapper à lire via l'entité de la BDD (VisiteurEntity)
            var entity = connection.QuerySingleOrDefault<VisiteurEntity>(sql, new { Email = email });

            // On vérifie tout de suite si l'entité est nulle pour rassurer le compilateur C#
            if (entity == null) 
            {
                return null;
            }

            // Ici, le compilateur sait à 100% que 'entity' n'est pas nul. 
            // On mappe de l'entité d'Infra vers l'objet 'Visiteur' de ton Core
            return new Visiteur
            {
                VisiteurId = entity.VisiteurId,
                VisiteurNom = entity.VisiteurNom,
                VisiteurPrenom = entity.VisiteurPrenom,
                VisiteurEmail = entity.VisiteurEmail,
                VisiteurMdp = entity.VisiteurMdp,
                VisiteurAdresse = entity.VisiteurAdresse,
                VisiteurCP = entity.VisiteurCP,
                VisiteurVille = entity.VisiteurVille,
                VisiteurPays = entity.VisiteurPays
            };
        }

        public void CreateVisiteur(Visiteur visiteur)
        {
            using var connection = GetConnection();

            string passwordHache = BCrypt.Net.BCrypt.HashPassword(visiteur.VisiteurMdp);

            var sql = @"
                INSERT INTO visiteur (VisiteurNom, VisiteurPrenom, VisiteurEmail, VisiteurMdp, VisiteurAdresse, VisiteurCP, VisiteurVille, VisiteurPays)
                VALUES (@Nom, @Prenom, @Email, @Password, @Adresse, @CP, @Ville, @Pays);";

            connection.Execute(sql, new
            {
                Nom = visiteur.VisiteurNom,
                Prenom = visiteur.VisiteurPrenom,
                Email = visiteur.VisiteurEmail,
                Password = passwordHache,
                Adresse = visiteur.VisiteurAdresse,
                CP = visiteur.VisiteurCP,
                Ville = visiteur.VisiteurVille,
                Pays = visiteur.VisiteurPays
            });
        }
    }
}