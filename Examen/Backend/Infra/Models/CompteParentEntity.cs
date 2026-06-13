namespace Infra.Models
{
    public class CompteParentEntity
    {
        // Dapper fera l'association automatiquement si les noms sont identiques à ta table MySQL
        public int CompteParentId { get; set; }
        public string NomPremierParent { get; set; } = string.Empty;
        public string PrenomPremierParent { get; set; } = string.Empty;
        public string MotDePasseCompte { get; set; } = string.Empty;
        public string EmailDeContact { get; set; } = string.Empty;
    }
}