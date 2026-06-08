namespace ListeDeNaissance.Core.Models;

public class ConsulterChoisir
{
    public int CompteParentId { get; set; }
    public CompteParent CompteParent { get; set; } = null!;

    public int ModelListeId { get; set; }
    public ModelDeListeDeNaissance ModelDeListeDeNaissance { get; set; } = null!;
}