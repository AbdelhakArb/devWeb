namespace Infra.Repositories.Abstractions;

public interface ICompteRepository
{
    // On pointe maintenant sur le bon modèle du Core : CompteParent
    global::ListeDeNaissance.Core.Models.CompteParent? GetCompteByEmail(string email);
}