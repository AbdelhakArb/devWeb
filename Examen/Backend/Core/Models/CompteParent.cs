namespace ListeDeNaissance.Core.Models;

public class CompteParent
{
    public int CompteParentId { get; set; }
    public string NomPremierParent { get; set; } = string.Empty;
    public string PrenomPremierParent { get; set; } = string.Empty;
    public string? NomSecondParent { get; set; }
    public string? PrenomSecondParent { get; set; }
    public string MotDePasseCompte { get; set; } = string.Empty;
    public string EmailDeContact { get; set; } = string.Empty;
    public string? AdresseParent { get; set; }
    public string? CpParent { get; set; }
    public string? VilleParent { get; set; }
    public string? PaysParent { get; set; }

    // Relations
    public List<ConsulterChoisir> ConsultationsModeles { get; set; } = new();
    public List<ListeDeNaissance> ListesDeNaissance { get; set; } = new();
}