using System.Collections.Generic;
using System.Threading.Tasks;
using ListeDeNaissance.Core.Models;

namespace ListeDeNaissance.Core.UseCases.Abstractions
{
    public interface IObtenirArticlesListeUseCase
    {
        Task<IEnumerable<Article>> ExecuterAsync(int listeId);
    }
}