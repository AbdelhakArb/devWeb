using ListeDeNaissance.Core.IGateways;
using ListeDeNaissance.Core.Usecases.Abstractions;

namespace ListeDeNaissance.Core.Usecases;

public class CreerListeDeNaissanceUseCase : ICreerListeDeNaissanceUseCase
{
    private readonly IListeDeNaissanceGateway _listeGateway;

    public CreerListeDeNaissanceUseCase(IListeDeNaissanceGateway listeGateway)
    {
        _listeGateway = listeGateway;
    }

    public async Task<Models.ListeDeNaissance> ExecuterAsync(Models.ListeDeNaissance nouvelleListe)
    {
        var listesDuParent = await _listeGateway.ObtenirListesParParentIdAsync(nouvelleListe.CompteParentId);
        
        bool nomExisteDeja = listesDuParent.Any(l => l.NomListeDeNaissance.Equals(nouvelleListe.NomListeDeNaissance, StringComparison.OrdinalIgnoreCase));
        
        if (nomExisteDeja)
        {
            throw new InvalidOperationException("Vous avez déjà créé une liste de naissance avec ce nom.");
        }

        nouvelleListe.DateCreationListe = DateTime.UtcNow;
        nouvelleListe.StatusListe = "Active";

        await _listeGateway.CreerListeAsync(nouvelleListe);
        return nouvelleListe;
    }
}
