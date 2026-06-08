namespace ListeDeNaissance.Core.Models;

public class Reservation
{
    public int ReservationId { get; set; }
    public int VisiteurId { get; set; }
    public Visiteur Visiteur { get; set; } = null!;

    public int PresenceArticleDansListeId { get; set; }
    public PresenceArticleDansListe PresenceArticleDansListe { get; set; } = null!;

    public int QtyReserve { get; set; }
    public DateTime? DateReservation { get; set; }
}