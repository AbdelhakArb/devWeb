using System;
using System.Threading.Tasks;
using ListeDeNaissance.Core.UseCases.Abstractions;
using ListeDeNaissance.Core.IGateways;
using ListeDeNaissance.Core.Models;

namespace ListeDeNaissance.Core.UseCases
{
    public class ConnexionVisiteurUseCase : IConnexionVisiteurUseCase
    {
        private readonly ICompteGateway _compteGateway;

        public ConnexionVisiteurUseCase(ICompteGateway compteGateway)
        {
            _compteGateway = compteGateway;
        }

        public async Task<Visiteur> ExecuterAsync(string email, string motDePasse)
        {
            var visiteur = await _compteGateway.VerifierConnexionVisiteurAsync(email, motDePasse);
            
            if (visiteur == null)
            {
                throw new UnauthorizedAccessException("Email ou mot de passe visiteur incorrect.");
            }

            return visiteur;
        }
    }
}