using Microsoft.Extensions.DependencyInjection;
using Infra.Repositories;
using Infra.Repositories.Abstractions;
using Infra.Gateway;
using ListeDeNaissance.Core.IGateways;

namespace Infra
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMyInfrastructureServices(this IServiceCollection services)
        {
            // --- BLOC A : Comptes & Authentification ---
            services.AddTransient<ICompteRepository, CompteRepository>();
            services.AddTransient<ICompteGateway, CompteGateway>();

            // --- BLOC B : Gestion des Listes de Naissance ---
            services.AddTransient<IListeDeNaissanceRepository, ListeDeNaissanceRepository>();
            services.AddTransient<IListeDeNaissanceGateway, ListeDeNaissanceGateway>();

            // --- BLOC C : Gestion des Articles ---
            services.AddTransient<IArticleRepository, ArticleRepository>();
            services.AddTransient<IArticleGateway, ArticleGateway>(); 

            return services;
        }
    }
}