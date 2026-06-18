using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Infra.Repositories.Abstractions;
using Infra.Models; 
using ListeDeNaissance.Core.Models;
using BCrypt.Net;
using System;
using System.Threading.Tasks;

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
        // 1. LOGIQUE PARENT (MODIFIÉE EN ASYNCHRONE)
        // =========================================================================

        public async Task<CompteParent?> GetCompteByEmailAsync(string email)
        {
            using var connection = GetConnection();
            var sql = "SELECT * FROM compteParent WHERE EmailDeContact = @Email";
            
            // Dapper va chercher la ligne et la mapper dans notre entité d'infrastructure
            var entity = await connection.QuerySingleOrDefaultAsync<CompteParentEntity>(sql, new { Email = email });

            if (entity == null) return null;

            // Mapping (Traduction) : On convertit l'entité de la BDD vers le modèle métier du Core
            return new CompteParent
            {
                CompteParentId = entity.CompteParentId,
                EmailDeContact = entity.EmailDeContact,
                MotDePasseCompte = entity.MotDePasseCompte,
                NomPremierParent = entity.NomPremierParent,
                PrenomPremierParent = entity.PrenomPremierParent
            };
        }

        public async Task CreateCompteAsync(CompteParent parent)
        {
            using var connection = GetConnection();

            // Sécurité : Hachage du mot de passe avec BCrypt avant insertion
            string passwordHache = BCrypt.Net.BCrypt.HashPassword(parent.MotDePasseCompte);

            var sql = @"
                INSERT INTO compteParent (EmailDeContact, MotDePasseCompte, NomPremierParent, PrenomPremierParent)
                VALUES (@Email, @Password, @Nom, @Prenom);";

            await connection.ExecuteAsync(sql, new
            {
                Email = parent.EmailDeContact,
                Password = passwordHache,   
                Nom = parent.NomPremierParent,     
                Prenom = parent.PrenomPremierParent 
            });
        }

        // =========================================================================
        // 2. LOGIQUE VISITEUR (MODIFIÉE EN ASYNCHRONE)
        // =========================================================================

        public async Task<Visiteur?> GetVisiteurByEmailAsync(string email)
        {
            using var connection = GetConnection();
            var sql = "SELECT * FROM visiteur WHERE VisiteurEmail = @Email";
            
            var entity = await connection.QuerySingleOrDefaultAsync<VisiteurEntity>(sql, new { Email = email });

            if (entity == null) return null;

            // Mapping (Traduction) : On convertit l'entité Visiteur de la BDD vers le modèle du Core
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

        public async Task CreateVisiteurAsync(Visiteur visiteur)
        {
            using var connection = GetConnection();

            string passwordHache = BCrypt.Net.BCrypt.HashPassword(visiteur.VisiteurMdp);

            var sql = @"
                INSERT INTO visiteur (VisiteurNom, VisiteurPrenom, VisiteurEmail, VisiteurMdp, VisiteurAdresse, VisiteurCP, VisiteurVille, VisiteurPays)
                VALUES (@Nom, @Prenom, @Email, @Password, @Adresse, @CP, @Ville, @Pays);";

            await connection.ExecuteAsync(sql, new
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