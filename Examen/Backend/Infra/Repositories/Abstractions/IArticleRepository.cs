namespace Infra.Repositories.Abstractions;

public interface IArticleRepository
{
    ListeDeNaissance.Core.Models.PresenceArticleDansListe? GetPresenceArticleById(int presenceArticleId);
    void CreateReservation(ListeDeNaissance.Core.Models.Reservation reservation);
}