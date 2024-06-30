using SimulationStorm.LiveChartsExtensions.FontManagement;
using SimulationStorm.LiveChartsExtensions.ThemeManagement.Options;

namespace SimulationStorm.LiveChartsExtensions;

public class LvcOptions
{
    public LvcFontsOptions FontsOptions { get; init; } = null!;

    public LvcThemesOptions ThemesOptions { get; init; } = null!;
}