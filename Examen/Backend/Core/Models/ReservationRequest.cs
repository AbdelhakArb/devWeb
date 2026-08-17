
namespace ListeDeNaissance.Core.Models
{
    public class ReservationRequestItem
    {
        public int ListeDeNaissanceId { get; set; }
        public int ArticleId { get; set; }
        public int QtySouhaitee { get; set; }
        public int VisiteurId { get; set; }
        public string? MessageText { get; set; }
        public string? SignatureMessage { get; set; }
    }
}