using System.Threading.Tasks;

namespace ListeDeNaissance.Core.UseCases.Abstractions
{
    public interface IIncrementerArticleListeUseCase
    {
        Task<int> ExecuterAsync(int listeId, int articleId);
    }
}