using System.Collections.Generic;
using System.Threading.Tasks;
using ListeDeNaissance.Core.Models;

namespace ListeDeNaissance.Core.UseCases.Abstractions
{
    public interface ISoumettreReservationsUseCase
    {
        Task<bool> ExecuterAsync(IEnumerable<ReservationRequestItem> panier);
    }
}