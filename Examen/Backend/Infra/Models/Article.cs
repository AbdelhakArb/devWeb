namespace Infra.Models
{
    public class Article
{
    public int ArticleId { get; set; }
    public string ArticleNom { get; set; } = string.Empty;
    public string? ArticleDesc { get; set; }
    public int? ArticleQty { get; set; }
    public decimal? ArticlePrix { get; set; }
}
}