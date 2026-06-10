using ListeDeNaissance.Core.Models;

namespace ListeDeNaissance.Core.Usecases.Abstractions;

public interface IReserverArticleUseCase
{
    Task<Reservation> ExecuterAsync(int presenceArticleId, int visiteurId, int quantiteAReserver);
}