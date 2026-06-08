using ListeDeNaissance.Core.Models;

namespace ListeDeNaissance.Core.IGateways;

public interface IListeDeNaissanceGateway
{
    Task CreerListeAsync(Models.ListeDeNaissance liste);
    Task<Models.ListeDeNaissance?> ObtenirListeParIdAsync(int listeId);
    Task<List<Models.ListeDeNaissance>> ObtenirListesParParentIdAsync(int compteParentId);
    Task<Models.ListeDeNaissance?> ObtenirListeParCodeOuLienAsync(string code);
    Task<List<ModelDeListeDeNaissance>> ObtenirTousLesModelesAsync();
}
