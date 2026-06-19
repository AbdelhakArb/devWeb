using System.Threading.Tasks;
using ListeDeNaissance.Core.Models;

namespace Infra.Repositories.Abstractions
{
    public interface ICompteRepository
    {
        // =========================================================================
        // 1. MÉTHODES POUR LES COMPTES PARENTS (ASYNCHRONES)
        // =========================================================================
        
        // Récupère un parent par son adresse email (Retourne null si aucun trouvé)
        Task<CompteParent?> GetCompteByEmailAsync(string email);
        
        // Crée un nouveau compte parent en base de données
        Task CreateCompteAsync(CompteParent parent);

        // =========================================================================
        // 2. MÉTHODES POUR LES VISITEURS (ASYNCHRONES)
        // =========================================================================
        
        // Récupère un visiteur par son adresse email (Retourne null si aucun trouvé)
        Task<Visiteur?> GetVisiteurByEmailAsync(string email);
        
        // Crée un nouveau compte visiteur en base de données
        Task CreateVisiteurAsync(Visiteur visiteur);
    }
}