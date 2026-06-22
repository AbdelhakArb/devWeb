using System;
using System.Threading.Tasks;
using ListeDeNaissance.Core.UseCases.Abstractions;
using ListeDeNaissance.Core.IGateways;
using ListeDeNaissance.Core.Models;

namespace ListeDeNaissance.Core.UseCases
{
    public class ConnexionParentUseCase : IConnexionParentUseCase
    {
        private readonly ICompteGateway _compteGateway;

        public ConnexionParentUseCase(ICompteGateway compteGateway)
        {
            _compteGateway = compteGateway;
        }

        public async Task<CompteParent> ExecuterAsync(string email, string motDePasse)
        {
            var parent = await _compteGateway.VerifierConnexionParentAsync(email, motDePasse);
            
            if (parent == null)
            {
                throw new UnauthorizedAccessException("Email ou mot de passe incorrect.");
            }

            return parent;
        }
    }
}