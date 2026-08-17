using Microsoft.Extensions.DependencyInjection;
using Infra.Repositories;
using Infra.Repositories.Abstractions;
using Infra.Gateways;
using ListeDeNaissance.Core.IGateways;
using Infra.Gateway;
using ListeDeNaissance.Core.UseCases.Abstractions;
using ListeDeNaissance.Core.UseCases;

namespace Infra
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMyInfrastructureServices(this IServiceCollection services)
        {
            services.AddTransient<ICompteRepository, CompteRepository>();
            services.AddTransient<ICompteGateway, CompteGateway>();

            services.AddTransient<IListeDeNaissanceRepository, ListeDeNaissanceRepository>();
            services.AddTransient<IListeDeNaissanceGateway, ListeDeNaissanceGateway>();
            
            services.AddTransient<IModelDeListeRepository, ModelDeListeRepository>();
            services.AddTransient<IModelDeListeGateway, ModelDeListeGateway>();
            
            services.AddTransient<IArticleRepository, ArticleRepository>();
            services.AddTransient<IArticleGateway, ArticleGateway>(); 
            return services;
        }
    }
}