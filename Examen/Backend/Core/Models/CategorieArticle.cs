namespace ListeDeNaissance.Core.Models;

public class CategorieArticle
{
    public int CategorieId { get; set; }
    public Categorie Categorie { get; set; } = null!;

    public int ArticleId { get; set; }
    public Article Article { get; set; } = null!;
}