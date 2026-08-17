using System;
using System.Threading.Tasks;
using ListeDeNaissance.Core.IGateways;
using ListeDeNaissance.Core.UseCases.Abstractions;

namespace ListeDeNaissance.Core.UseCases
{
    public class AjouterArticleDansListeUseCase : IAjouterArticleDansListeUseCase
    {
        private readonly IListeDeNaissanceGateway _listeDeNaissanceGateway;

        public AjouterArticleDansListeUseCase(IListeDeNaissanceGateway listeDeNaissanceGateway)
        {
            _listeDeNaissanceGateway = listeDeNaissanceGateway;
        }

        public async Task ExecuterAsync(int listeId, int articleId, int quantite)
        {
            if (listeId <= 0 || articleId <= 0 || quantite <= 0)
            {
                throw new ArgumentException("Les données de l'article ou de la liste sont invalides.");
            }

            await _listeDeNaissanceGateway.AjouterArticleDansListeAsync(listeId, articleId, quantite);
        }
    }
}