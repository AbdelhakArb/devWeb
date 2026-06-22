using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ListeDeNaissance.Core.IGateways;
using ListeDeNaissance.Core.UseCases.Abstractions;
using ListeDeNaissance.Core.Models;

namespace ListeDeNaissance.Core.UseCases.Abstractions
{
    public interface IObtenirArticlesUseCase
    {
        Task<IEnumerable<Article>> ObtenirCatalogueArticlesAsync();
    }  
} 
   