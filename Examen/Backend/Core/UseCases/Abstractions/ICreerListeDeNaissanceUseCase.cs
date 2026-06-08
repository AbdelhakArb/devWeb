using ListeDeNaissance.Core.Models;

namespace ListeDeNaissance.Core.Usecases.Abstractions;

public interface ICreerListeDeNaissanceUseCase
{
    Task<Models.ListeDeNaissance> ExecuterAsync(Models.ListeDeNaissance nouvelleListe);
}
