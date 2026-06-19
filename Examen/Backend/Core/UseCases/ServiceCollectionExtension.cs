using Microsoft.Extensions.DependencyInjection;
using ListeDeNaissance.Core.UseCases;
using ListeDeNaissance.Core.UseCases.Abstractions;

namespace ListeDeNaissance.Core;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddMonAppliCoreServices(this IServiceCollection services)
    {
        // =========================================================================
        // BLOC A : USECASE AUTHENTIFICATION & COMPTES
        // =========================================================================
        services.AddScoped<IAuthentificationUseCase, AuthentificationUseCase>();

        // =========================================================================
        // BLOC B : USECASE GESTION DES LISTES DE NAISSANCE
        // =========================================================================
        services.AddScoped<IGestionListeUseCase, GestionListeUseCase>();

        return services;
    }
}