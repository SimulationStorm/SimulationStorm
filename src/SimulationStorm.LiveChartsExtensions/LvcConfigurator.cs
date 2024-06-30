using System;
using LiveChartsCore;
using SimulationStorm.LiveChartsExtensions.FontManagement;
using SimulationStorm.LiveChartsExtensions.ThemeManagement;

namespace SimulationStorm.LiveChartsExtensions;

public class LvcConfigurator : IDisposable
{
    public LvcConfigurator(LvcOptions options) => LiveCharts.DefaultSettings.Configure(options);

    public void Dispose()
    {
        LvcFontManager.Instance.Dispose();
        LvcThemeManager.Instance.Dispose();
        GC.SuppressFinalize(this);
    }
}