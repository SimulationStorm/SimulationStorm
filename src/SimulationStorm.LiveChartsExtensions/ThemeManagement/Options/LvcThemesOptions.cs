using System.Collections.Generic;

namespace SimulationStorm.LiveChartsExtensions.ThemeManagement.Options;

public class LvcThemesOptions
{
    public IReadOnlyDictionary<LvcTheme, LvcThemeOptions> ThemeOptionsByTheme { get; init; } = null!;

    public LvcTheme DefaultTheme { get; init; }
}