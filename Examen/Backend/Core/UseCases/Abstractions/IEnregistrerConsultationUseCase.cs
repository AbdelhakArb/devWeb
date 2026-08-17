using System.Threading.Tasks;

namespace ListeDeNaissance.Core.UseCases.Abstractions
{
    public interface IEnregistrerConsultationUseCase
    {
        Task ExecuterAsync(int listeId, int visiteurId);
    }
}