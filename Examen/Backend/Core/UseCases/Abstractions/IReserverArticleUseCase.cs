using ListeDeNaissance.Core.Models;

namespace ListeDeNaissance.Core.UseCases.Abstractions;

public interface IReserverArticleUseCase
{
    Task<Reservation> ExecuterAsync(int presenceArticleId, int visiteurId, int quantiteAReserver);
}