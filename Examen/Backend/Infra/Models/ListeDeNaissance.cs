namespace Infra.Models
{
    public class ListeDeNaissance
    {
        public int ListeDeNaissanceId { get; set; } 
        public int CompteParentId { get; set; }
        public string NomListeDeNaissance { get; set; } = string.Empty;
        public DateTime DateCreationListe { get; set; }
        public DateTime DatePrevuPourAccouchement { get; set; }
        public string StatusListe { get; set; } = string.Empty;
        public string? LieuListe { get; set; }
        public string? CodeUniqueListe { get; set; }
    }
}