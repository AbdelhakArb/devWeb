using System.Threading.Tasks;
using ListeDeNaissance.Core.Models;

namespace ListeDeNaissance.Core.UseCases.Abstractions
{
    public interface IAfficherListeParIdUseCase
    {
        Task<ListeDeNaissance.Core.Models.ListeDeNaissance?> ExecuterAsync(int id);
    }
}