using System;
using System.Collections.Generic;
using LiveChartsCore;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView;

namespace SimulationStorm.LiveChartsExtensions.ThemeManagement;

public class LvcThemeManager : IDisposable
{
    public static readonly LvcThemeManager Instance = new();
    
    public LvcTheme? CurrentTheme { get; private set; }

    public event EventHandler? ThemeChanged;

    private readonly IDictionary<LvcTheme, Action<LiveChartsSettings>> _themeBuildersByTheme =
        new Dictionary<LvcTheme, Action<LiveChartsSettings>>();

    private LvcThemeManager() => RegisterDefaultThemeBuilders();

    #region Public methods
    public void RegisterThemeBuilder(LvcTheme theme, Action<LiveChartsSettings> themeBuilder) =>
        _themeBuildersByTheme[theme] = themeBuilder;

    public void ChangeTheme(LvcTheme newTheme)
    {
        if (newTheme == CurrentTheme)
            return;

        var themeBuilder = _themeBuildersByTheme[newTheme];
        LiveCharts.DefaultSettings.DisposeLegendAndTooltipPaints();
        themeBuilder(LiveCharts.DefaultSettings);
        
        CurrentTheme = newTheme;
        ThemeChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Dispose()
    {
        LiveCharts.DefaultSettings.DisposeLegendAndTooltipPaints();
        GC.SuppressFinalize(this);
    }
    #endregion

    private void RegisterDefaultThemeBuilders()
    {
        _themeBuildersByTheme[LvcTheme.Dark] = settings => settings.AddDarkTheme();
        _themeBuildersByTheme[LvcTheme.Light] = settings => settings.AddLightTheme();
    }
}