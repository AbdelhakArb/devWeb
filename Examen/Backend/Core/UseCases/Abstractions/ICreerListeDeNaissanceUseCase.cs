using ListeDeNaissance.Core.Models;
using System.Threading.Tasks;


namespace ListeDeNaissance.Core.UseCases.Abstractions;

public interface ICreerListeDeNaissanceUseCase
{
    Task<Models.ListeDeNaissance> ExecuterAsync(Models.ListeDeNaissance nouvelleListe);
}
