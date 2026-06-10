namespace ListeDeNaissance.Core.Models;

public class Messages
{
    public int MessageId { get; set; }
    public int ListeDeNaissanceId { get; set; }
    public ListeDeNaissance ListeDeNaissance { get; set; } = null!;

    public int VisiteurId { get; set; }
    public Visiteur Visiteur { get; set; } = null!;

    public string MessageText { get; set; } = string.Empty;
    public string? SignatureMessage { get; set; }
    public DateTime DateMessage { get; set; }
}
