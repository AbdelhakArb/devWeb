using System.Threading.Tasks;
using ListeDeNaissance.Core.Models;

namespace ListeDeNaissance.Core.UseCases.Abstractions
{
    public interface IAjouterArticleDansListeUseCase
    {
        Task ExecuterAsync(PresenceArticleDansListe presenceArticle);
    }
}