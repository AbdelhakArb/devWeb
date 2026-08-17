using Microsoft.Extensions.DependencyInjection;
using ListeDeNaissance.Core.UseCases;
using ListeDeNaissance.Core.UseCases.Abstractions;

namespace ListeDeNaissance.Core;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddMonAppliCoreServices(this IServiceCollection services)
    {
        // Auth & Inscription
        services.AddScoped<IConnexionParentUseCase, ConnexionParentUseCase>();
        services.AddScoped<IConnexionVisiteurUseCase, ConnexionVisiteurUseCase>();
        services.AddScoped<IInscrireParentUseCase, InscrireParentUseCase>();
        services.AddScoped<IInscrireVisiteurUseCase, InscrireVisiteurUseCase>();

        // Gestion Liste et Models
        services.AddScoped<ICreerListeDeNaissanceUseCase, CreerListeDeNaissanceUseCase>();
        services.AddScoped<IAfficherListesParentParParentIdUseCase, AfficherListesParentParParentIdUseCase>(); 
        services.AddScoped<IAfficherListeParIdUseCase, AfficherListeParIdUseCase>();
        services.AddScoped<IObtenirTousLesModelsUseCase, ObtenirTousLesModelsUseCase>();
        services.AddScoped<IObtenirArticlesDuModelUseCase, ObtenirArticlesDuModelUseCase>();
        
        // Articles
        services.AddScoped<IAjouterArticleDansListeUseCase, AjouterArticleDansListeUseCase>();
        services.AddScoped<IIncrementerArticleListeUseCase, IncrementerArticleListeUseCase>();
        services.AddScoped<IDecrementerArticleListeUseCase, DecrementerArticleListeUseCase>();
        services.AddScoped<IObtenirArticlesListeUseCase, ObtenirArticlesListeUseCase>();
        services.AddScoped<IObtenirArticlesUseCase, ObtenirArticlesUseCase>();
        
        // Réservations
        services.AddScoped<IReserverArticleUseCase, ReserverArticleUseCase>();
        services.AddScoped<ISoumettreReservationsUseCase, SoumettreReservationsUseCase>();
        // Consultation
        services.AddScoped<IEnregistrerConsultationUseCase, EnregistrerConsultationUseCase>();
        // Cloturer Liste
        services.AddScoped<ICloturerListeUseCase, CloturerListeUseCase>();

        return services;
    }
}