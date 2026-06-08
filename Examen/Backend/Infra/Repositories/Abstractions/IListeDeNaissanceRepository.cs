using Infra.Models;

namespace Infra.Repositories.Abstractions;

public interface IListeDeNaissanceRepository
{
    ListeDeNaissance? GetListeById(int listeId);
    IEnumerable<ListeDeNaissance> GetListesByParentId(int compteParentId);
    void CreateListe(ListeDeNaissance liste);
}