using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.Avalonia.Colors;
using SimulationStorm.Avalonia.TimeFormatting;
using SimulationStorm.Densities.Avalonia;
using SimulationStorm.LiveChartsExtensions.Avalonia;
using SimulationStorm.Themes.Avalonia;
using SimulationStorm.Threading.Avalonia;

namespace SimulationStorm.GameOfLife.Avalonia.Startup;

public static class AvaloniaDependencyInjection
{
    public static IServiceCollection AddAvaloniaServices(this IServiceCollection services) => services
        .AddThreadingServices()
        .AddUiThemeManager()
        .AddUiDensityManager()
        .AddUiColorProvider()
        .AddTimeFormattingServices()
        .AddLiveCharts();
}