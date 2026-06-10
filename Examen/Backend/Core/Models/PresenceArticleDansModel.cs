namespace ListeDeNaissance.Core.Models;

public class PresenceArticleDansModel
{
    public int ModelListeId { get; set; }
    public ModelDeListeDeNaissance ModelDeListeDeNaissance { get; set; } = null!;

    public int ArticleId { get; set; }
    public Article Article { get; set; } = null!;
}
