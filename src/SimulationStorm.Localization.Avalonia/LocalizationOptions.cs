using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SimulationStorm.Localization.Avalonia;

public class LocalizationOptions
{
    public bool SetInitialCultureToDefault { get; init; }

    public CultureInfo DefaultCulture { get; init; } = null!;

    public IEnumerable<CultureInfo> SupportedCultures { get; init; } = [];

    public IEnumerable<string> AssembliesContainingLocaleResources { get; init; } = [];

    public string LocaleResourcesDirectoryPath { get; init; } = string.Empty;

    public string StringResourceKeyPrefix { get; init; } = string.Empty;
}