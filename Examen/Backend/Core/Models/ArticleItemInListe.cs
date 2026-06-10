namespace ListeDeNaissance.Core.Models;

public class ArticleItemInListe
{
    public int PresenceArticleListeId { get; set; }
    public int ListeDeNaissanceId { get; set; }
    public int QtySouhaitee { get; set; }
    public int QtyReservee { get; set; }
    public int QtyRestante { get; set; }
    public ArticleDto Article { get; set; } = new();
}

public class ArticleDto
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public decimal Prix { get; set; }
}