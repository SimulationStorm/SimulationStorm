using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Markup.Xaml.Styling;

namespace SimulationStorm.Localization.Avalonia;

/// <inheritdoc/>
public class LocaleResourceProvider : ILocaleResourceProvider
{
    /// <inheritdoc/>
    public IEnumerable<CultureInfo> AvailableCultures => _localeResourcesByCulture.Keys;

    private readonly IReadOnlyDictionary<CultureInfo, IEnumerable<ResourceInclude>> _localeResourcesByCulture;

    public LocaleResourceProvider(IReadOnlyDictionary<CultureInfo, IEnumerable<Uri>> localeResourceUrisByCulture)
    {
        _localeResourcesByCulture = localeResourceUrisByCulture.ToDictionary
        (
            kv => kv.Key,
            kv => kv.Value.Select(resourceUri => new ResourceInclude(resourceUri) { Source = resourceUri })
        );
    }

    /// <inheritdoc/>
    public IEnumerable<ResourceInclude> GetLocaleResourcesByCulture(CultureInfo culture)
    {
        if (!_localeResourcesByCulture.TryGetValue(culture, out var localeResource))
            throw new NotSupportedException($"The culture {culture} is not supported.");

        return localeResource;
    }

    public static LocaleResourceProvider FromCulturesAndAssemblyNames
    (
        IEnumerable<CultureInfo> cultures,
        IEnumerable<string> assemblyNames,
        string localeResourcesDirectoryPath)
    {
        const string localeResourceUriPattern = "avares://{0}/{1}/{2}.axaml";
        
        var localeResourceUrisByCulture = cultures.ToDictionary
        (
            culture => culture,
            culture => assemblyNames
                .Select(assemblyName =>
                    string.Format(localeResourceUriPattern, assemblyName, localeResourcesDirectoryPath, culture.Name))
                .Select(uriString => new Uri(uriString))
        );

        return new LocaleResourceProvider(localeResourceUrisByCulture);
    }
}