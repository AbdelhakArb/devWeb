namespace ListeDeNaissance.Core.Models;

public class ModelDeListeDeNaissance
{
    public int ModelListeId { get; set; }
    public string ModelListeNom { get; set; } = string.Empty;
    public string? ModelListeDesc { get; set; }

    // Relations
    public List<ConsulterChoisir> ConsultationsParents { get; set; } = new();
    public List<PresenceArticleDansModel> PresenceArticles { get; set; } = new();
}
