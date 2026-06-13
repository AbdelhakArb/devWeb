namespace ListeDeNaissance.Core.Models;

public class PanierReservationDto
{
    public int VisiteurId { get; set; }
    public List<LigneReservationDto> ListeReservations { get; set; } = new();
}

public class LigneReservationDto
{
    public int PresenceArticleListeId { get; set; }
    public int QtyReserve { get; set; }
}
