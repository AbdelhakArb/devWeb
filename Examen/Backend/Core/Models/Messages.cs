namespace ListeDeNaissance.Core.Models;

public class Messages
{
    public int MessageId { get; set; }
    public int ListeDeNaissanceId { get; set; }
    public int VisiteurId { get; set; }
    public string MessageText { get; set; } = string.Empty;
    public string? SignatureMessage { get; set; }
}
