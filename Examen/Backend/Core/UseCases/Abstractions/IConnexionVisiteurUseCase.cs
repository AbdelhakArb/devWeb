using System.Threading.Tasks;
using ListeDeNaissance.Core.Models;

namespace ListeDeNaissance.Core.UseCases.Abstractions
{
    public interface IConnexionVisiteurUseCase
    {
        Task<Visiteur> ExecuterAsync(string email, string motDePasse);
    }
}