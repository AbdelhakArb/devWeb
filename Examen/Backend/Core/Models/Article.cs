namespace ListeDeNaissance.Core.Models;

public class Article
{
    public int ArticleId { get; set; }
    public string ArticleNom { get; set; } = string.Empty;
    public string? ArticleDesc { get; set; }
    public int? ArticleQty { get; set; }
    public decimal? ArticlePrix { get; set; }
    public int QtyReserve { get; set; }   // ← ajoutée : quantité déjà réservée par les visiteurs

    // Relations
    public List<CategorieArticle> CategorieArticles { get; set; } = new();
    public List<PresenceArticleDansListe> PresenceDansListes { get; set; } = new();
    public List<PresenceArticleDansModel> PresenceDansModels { get; set; } = new();
}