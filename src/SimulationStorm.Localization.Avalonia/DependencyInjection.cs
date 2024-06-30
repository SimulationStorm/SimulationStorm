using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.AppStates.Presentation;
using SimulationStorm.Localization.Presentation;
using SimulationStorm.Localization.States;

namespace SimulationStorm.Localization.Avalonia;

public static class LocalizationDependencyInjection
{
    public static IServiceCollection AddLocalizationManager
    (
        this IServiceCollection services,
        IResourceDictionary localeResourceDictionary,
        LocalizationOptions options)
    {
        return services
            .AddSingleton<ILocalizationManager>(_ =>
            {
                var localeResourceProvider = LocaleResourceProvider.FromCulturesAndAssemblyNames
                (
                    options.SupportedCultures,
                    options.AssembliesContainingLocaleResources,
                    options.LocaleResourcesDirectoryPath
                );

                return new LocalizationManager(localeResourceDictionary, localeResourceProvider, options);
            })
            .AddServiceStateManager<LocalizationStateManager>();
    }
}