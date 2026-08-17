using System.Threading.Tasks;

namespace ListeDeNaissance.Core.UseCases.Abstractions
{
    public interface IAjouterArticleDansListeUseCase
    {
        Task ExecuterAsync(int listeId, int articleId, int quantite);
    }
}