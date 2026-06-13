namespace Infra.Repositories.Abstractions
{
    public interface ICompteRepository
    {
        // =========================================================================
        // 1. MÉTHODES POUR LES COMPTES PARENTS
        // =========================================================================
        
        // Récupère un parent par son adresse email (Retourne null si aucun trouvé)
        global::ListeDeNaissance.Core.Models.CompteParent? GetCompteByEmail(string email);
        
        // Crée un nouveau compte parent en base de données
        void CreateCompte(global::ListeDeNaissance.Core.Models.CompteParent parent);

        // =========================================================================
        // 2. MÉTHODES POUR LES VISITEURS
        // =========================================================================
        
        // Récupère un visiteur par son adresse email (Retourne null si aucun trouvé)
        global::ListeDeNaissance.Core.Models.Visiteur? GetVisiteurByEmail(string email);
        
        // Crée un nouveau compte visiteur en base de données
        void CreateVisiteur(global::ListeDeNaissance.Core.Models.Visiteur visiteur);
    }
}