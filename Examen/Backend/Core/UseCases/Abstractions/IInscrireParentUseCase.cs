using System.Threading.Tasks;
using ListeDeNaissance.Core.Models;

namespace ListeDeNaissance.Core.UseCases.Abstractions
{
    public interface IInscrireParentUseCase
    {
        Task ExecuterAsync(CompteParent parent);
    }
}