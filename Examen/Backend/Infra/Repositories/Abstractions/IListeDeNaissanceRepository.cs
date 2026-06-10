namespace Infra.Repositories.Abstractions;

public interface IListeDeNaissanceRepository
{
    Task CreerListeAsync(global::ListeDeNaissance.Core.Models.ListeDeNaissance liste);
    Task<global::ListeDeNaissance.Core.Models.ListeDeNaissance?> ObtenirListeParIdAsync(int listeId);
    Task<List<global::ListeDeNaissance.Core.Models.ListeDeNaissance>> ObtenirListesParParentIdAsync(int compteParentId);
}