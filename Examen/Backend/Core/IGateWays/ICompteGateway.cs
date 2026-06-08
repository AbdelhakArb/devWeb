using ListeDeNaissance.Core.Models;

namespace ListeDeNaissance.Core.IGateways;

public interface ICompteGateway
{
    Task<CompteParent?> ObtenirParentParEmailAsync(string email);
    Task<Visiteur?> ObtenirVisiteurParEmailAsync(string email);
    Task CreerCompteParentAsync(CompteParent parent);
    Task CreerCompteVisiteurAsync(Visiteur visiteur);
}