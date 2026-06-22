namespace Infra.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public int QuantiteReservee { get; set; }
        public string NomVisiteur { get; set; } = string.Empty;
        public string MessageVisiteur { get; set; } = string.Empty;
        public string StatusReservation { get; set; } = string.Empty;
        public int PresenceArticleDansListeId { get; set; }
        public PresenceArticleDansListe PresenceArticleDansListe { get; set; } = null!;
    }
}