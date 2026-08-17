using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infra.Repositories.Abstractions
{
    public interface IArticleRepository
    {
        Infra.Models.PresenceArticleDansListe? GetPresenceArticleById(int presenceArticleId);
        void CreateReservation(Infra.Models.Reservation reservation);
        Task<IEnumerable<Infra.Models.Article>> ObtenirTousLesArticlesAsync();
        Task<Infra.Models.Article?> ObtenirArticleParIdAsync(int articleId);
    }
}