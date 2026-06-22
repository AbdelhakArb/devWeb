using ListeDeNaissance.Core.IGateways;
using ListeDeNaissance.Core.Models;
using ListeDeNaissance.Core.UseCases.Abstractions;

namespace ListeDeNaissance.Core.UseCases;

public class ReserverArticleUseCase : IReserverArticleUseCase
{
    private readonly IArticleGateway _articleGateway;

    public ReserverArticleUseCase(IArticleGateway articleGateway)
    {
        _articleGateway = articleGateway;
    }

    public async Task<Reservation> ExecuterAsync(int presenceArticleId, int visiteurId, int quantiteAReserver)
    {
        var presenceArticle = await _articleGateway.ObtenirPresenceArticleAsync(presenceArticleId);
        if (presenceArticle == null)
        {
            throw new KeyNotFoundException("Cet article n'existe pas dans la liste.");
        }

        int totalDejaReserve = presenceArticle.Reservations?.Sum(r => r.QtyReserve) ?? 0;
        int quantiteDisponible = presenceArticle.QtySouhaitee - totalDejaReserve;

        if (quantiteAReserver > quantiteDisponible)
        {
            throw new InvalidOperationException($"Action impossible. Il ne reste que {quantiteDisponible} article(s) disponible(s) sur les {presenceArticle.QtySouhaitee} demandés.");
        }

        var nouvelleReservation = new Reservation
        {
            VisiteurId = visiteurId,
            PresenceArticleDansListeId = presenceArticleId,
            QtyReserve = quantiteAReserver,
            DateReservation = DateTime.UtcNow
        };

        await _articleGateway.CreerReservationAsync(nouvelleReservation);
        return nouvelleReservation;
    }
}