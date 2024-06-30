using System.Collections.Generic;

namespace SimulationStorm.LiveChartsExtensions.FontManagement;

public class LvcFontsOptions
{
    public string FontResourcesAssemblyName { get; init; } = null!;

    public string FontResourcesDirectoryPath { get; init; } = null!;

    public string FontResourceExtension { get; init; } = null!;

    public IReadOnlyDictionary<string, LvcFont> FontsByFontResourceNames { get; init; } = null!;

    public LvcFont DefaultFont { get; init; }
}