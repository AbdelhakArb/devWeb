namespace ListeDeNaissance.Core.Models;

public class Consultation
{
    public int ListeDeNaissanceId { get; set; }
    public ListeDeNaissance ListeDeNaissance { get; set; } = null!;

    public int VisiteurId { get; set; }
    public Visiteur Visiteur { get; set; } = null!;

    public DateTime DateConsultation { get; set; }
}