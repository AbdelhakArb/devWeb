namespace ListeDeNaissance.Core.Models;

public class ListeDeNaissance
{
    public int ListeDeNaissanceId { get; set; }
    public int CompteParentId { get; set; }
    public CompteParent CompteParent { get; set; } = null!;

    public string NomListeDeNaissance { get; set; } = string.Empty;
    public DateTime DateCreationListe { get; set; }
    public DateTime DatePrevuPourAccouchement { get; set; }
    public string StatusListe { get; set; } = string.Empty;
    public string? LieuListe { get; set; }

    // Relations
    public List<Consultation> Consultations { get; set; } = new();
    public List<Messages> Messages { get; set; } = new();
    public List<PresenceArticleDansListe> PresenceArticles { get; set; } = new();
}