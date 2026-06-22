using System;
using System.Threading.Tasks;
using ListeDeNaissance.Core.Models;
using ListeDeNaissance.Core.IGateways;
using ListeDeNaissance.Core.UseCases.Abstractions;

namespace ListeDeNaissance.Core.UseCases
{
    public class InscrireParentUseCase : IInscrireParentUseCase
    {
        private readonly ICompteGateway _compteGateway;

        public InscrireParentUseCase(ICompteGateway compteGateway)
        {
            _compteGateway = compteGateway;
        }

        public async Task ExecuterAsync(CompteParent parent)
        {
            if (parent == null) throw new ArgumentNullException(nameof(parent));

            // C'est ici que tu pourrais ajouter des règles métier SOLID (ex: vérifier le format de l'email)
            await _compteGateway.InscrireParentAsync(parent);
        }
    }
}