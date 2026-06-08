namespace ListeDeNaissance.Core.Models;

public class Visiteur
{
    public int VisiteurId { get; set; }
    public string VisiteurNom { get; set; } = string.Empty;
    public string VisiteurPrenom { get; set; } = string.Empty;
    public string VisiteurEmail { get; set; } = string.Empty;
    public string VisiteurMdp { get; set; } = string.Empty;
    public string? VisiteurAdresse { get; set; }
    public string? VisiteurCP { get; set; }
    public string? VisiteurVille { get; set; }
    public string? VisiteurPays { get; set; }

    // Relations
    public List<Consultation> Consultations { get; set; } = new();
    public List<Messages> Messages { get; set; } = new();
    public List<Reservation> Reservations { get; set; } = new();
}
