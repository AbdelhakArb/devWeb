using System.Threading.Tasks;

namespace ListeDeNaissance.Core.UseCases.Abstractions
{
    public interface ICloturerListeUseCase
    {
        Task ExecuterAsync(int listeId, string statusListe);
    }
}