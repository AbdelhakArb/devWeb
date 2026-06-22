namespace Infra.Repositories.Abstractions;

public interface IArticleRepository
{
    Infra.Models.PresenceArticleDansListe? GetPresenceArticleById(int presenceArticleId);
    void CreateReservation(Infra.Models.Reservation reservation);
    Task<IEnumerable<Infra.Models.Article>> ObtenirTousLesArticlesAsync();

}