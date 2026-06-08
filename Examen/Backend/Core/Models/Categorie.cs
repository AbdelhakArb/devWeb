namespace ListeDeNaissance.Core.Models;

public class Categorie
{
    public int CategorieId { get; set; }
    public string CategorieNom { get; set; } = string.Empty;
    public string? CategorieDesc { get; set; }

    // Relations
    public List<CategorieArticle> CategorieArticles { get; set; } = new();
}