namespace Infra.Models
{
    public class PresenceArticleDansListe
{
    public int PresenceArticleDansListeId { get; set; }
    public int ListeDeNaissanceId { get; set; }
    public ListeDeNaissance ListeDeNaissance { get; set; } = null!;

    public int ArticleId { get; set; }
    public Article Article { get; set; } = null!;

    public int QtySouhaitee { get; set; }
}
}
