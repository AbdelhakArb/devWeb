using System.Threading.Tasks;
using ListeDeNaissance.Core.Models;

namespace ListeDeNaissance.Core.UseCases.Abstractions
{
    public interface IConnexionParentUseCase
    {
        Task<CompteParent> ExecuterAsync(string email, string motDePasse);
    }
}