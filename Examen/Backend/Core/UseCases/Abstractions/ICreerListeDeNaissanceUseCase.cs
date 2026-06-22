using ListeDeNaissance.Core.Models;
using System.Threading.Tasks;


namespace ListeDeNaissance.Core.Usecases.Abstractions;

public interface ICreerListeDeNaissanceUseCase
{
    Task<Models.ListeDeNaissance> ExecuterAsync(Models.ListeDeNaissance nouvelleListe);
}
