using System;

namespace Infra.Models
{
    public class ListeDeNaissance
    {
        public int IdListeDeNaissance { get; set; }
        public string NomListeDeNaissance { get; set; } = string.Empty;
        public DateTime DateCreationListe { get; set; }
        public string StatusListe { get; set; } = string.Empty;
        public int CompteParentId { get; set; }
        public string? CodeUniqueListe { get; set; } 
}
}