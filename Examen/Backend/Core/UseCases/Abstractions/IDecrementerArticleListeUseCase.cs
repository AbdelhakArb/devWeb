using System.Threading.Tasks;

namespace ListeDeNaissance.Core.UseCases.Abstractions
{
    public interface IDecrementerArticleListeUseCase
    {
        Task<int> ExecuterAsync(int listeId, int articleId);
    }
}