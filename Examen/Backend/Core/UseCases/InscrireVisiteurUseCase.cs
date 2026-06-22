using System;
using System.Threading.Tasks;
using ListeDeNaissance.Core.Models;
using ListeDeNaissance.Core.IGateways;
using ListeDeNaissance.Core.UseCases.Abstractions;

namespace ListeDeNaissance.Core.UseCases
{
    public class InscrireVisiteurUseCase : IInscrireVisiteurUseCase
    {
        private readonly ICompteGateway _compteGateway;

        public InscrireVisiteurUseCase(ICompteGateway compteGateway)
        {
            _compteGateway = compteGateway;
        }

        public async Task ExecuterAsync(Visiteur visiteur)
        {
            if (visiteur == null) throw new ArgumentNullException(nameof(visiteur));

            await _compteGateway.InscrireVisiteurAsync(visiteur);
        }
    }
}