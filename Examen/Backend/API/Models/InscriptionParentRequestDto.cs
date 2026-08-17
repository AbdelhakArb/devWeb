namespace ListeDeNaissance.API.DTOs
{
    public class InscriptionParentRequestDto
    {
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MotDePasse { get; set; } = string.Empty;
        public string? Adresse { get; set; }
        public string? Cp { get; set; }
        public string? Ville { get; set; }
        public string? Pays { get; set; }
    }
}