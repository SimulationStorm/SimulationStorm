using System;
using System.Linq;
using System.Reflection;
using LiveChartsCore;
using LiveChartsCore.Kernel;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.Painting.ImageFilters;
using LiveChartsCore.Themes;
using SimulationStorm.LiveChartsExtensions.FontManagement;
using SimulationStorm.LiveChartsExtensions.ThemeManagement;
using SimulationStorm.LiveChartsExtensions.ThemeManagement.Options;
using SkiaSharp;

namespace SimulationStorm.LiveChartsExtensions;

public static class LvcSettingsExtensions
{
    public static LiveChartsSettings Configure(this LiveChartsSettings settings, LvcOptions options)
    {
        settings
            .AddSkiaSharp()
            .AddDefaultMappers()
            .ConfigureFonts(options.FontsOptions)
            .ConfigureThemes(options.ThemesOptions);

        return settings;
    }

    public static LiveChartsSettings ConfigureFonts(this LiveChartsSettings settings, LvcFontsOptions options)
    {
        var assembly = Assembly.Load(options.FontResourcesAssemblyName);
        var fontResourcesDirectoryPath = options.FontResourcesDirectoryPath.Replace('/', '.');
        var fontResourcesDirectoryAbsolutePath = $"{options.FontResourcesAssemblyName}.{fontResourcesDirectoryPath}";

        foreach (var (fontResourceName, font) in options.FontsByFontResourceNames)
        {
            var fontResourcePath = $"{fontResourcesDirectoryAbsolutePath}.{fontResourceName}.{options.FontResourceExtension}";
            var fontResourceStream = assembly.GetManifestResourceStream(fontResourcePath);
            var typeface = SKTypeface.FromStream(fontResourceStream);
            
            LvcFontManager.Instance.RegisterTypeface(font, typeface);
        }

        // LvcFontManager.Instance.DefaultTypeface = LvcFontManager.Instance.GetTypeface(options.DefaultFont);
        LiveCharts.DefaultSettings.HasGlobalSKTypeface(LvcFontManager.Instance.GetTypeface(options.DefaultFont));

        return settings;
    }

    public static LiveChartsSettings ConfigureThemes(this LiveChartsSettings settings, LvcThemesOptions options)
    {
        LvcThemeManager.Instance.RegisterThemeBuilder(LvcTheme.Dark, stgs =>
            stgs.ConfigureTheme(options.ThemeOptionsByTheme[LvcTheme.Dark]));
        
        LvcThemeManager.Instance.RegisterThemeBuilder(LvcTheme.Light, stgs =>
            stgs.ConfigureTheme(options.ThemeOptionsByTheme[LvcTheme.Light]));
        
        LvcThemeManager.Instance.ChangeTheme(options.DefaultTheme);

        return settings;
    }

    public static LiveChartsSettings ConfigureTheme(this LiveChartsSettings settings, LvcThemeOptions options)
    {
        LiveCharts.HasTheme = true;

        return settings
            .WithAnimationsSpeed(options.AnimationsSpeed)
            .WithEasingFunction(options.EasingFunction)
            .ConfigureLegend(options.LegendOptions)
            .ConfigureTooltip(options.TooltipOptions)
            .HasTheme<SkiaSharpDrawingContext>(theme => theme
                .WithColors(options.ColorPalette.ToArray())
                .ConfigureRules(options));
    }
    
    public static LiveChartsSettings ConfigureLegend(this LiveChartsSettings settings, LvcLegendOptions options) => settings
        .WithLegendTextPaint(LvcHelpers.CreateTextPaint(options.Font, options.ForegroundColor))
        .WithLegendTextSize<SkiaSharpDrawingContext>(options.FontSize)
        .WithLegendBackgroundPaint(LvcHelpers.CreateFillPaint(options.BackgroundColor));

    public static LiveChartsSettings ConfigureTooltip(this LiveChartsSettings settings, LvcTooltipOptions options)
    {
        settings
            .WithTooltipTextPaint(LvcHelpers.CreateTextPaint(options.Font, options.ForegroundColor))
            .WithTooltipTextSize<SkiaSharpDrawingContext>(options.FontSize);

        var backgroundPaint = LvcHelpers.CreateFillPaint(options.BackgroundColor);
        backgroundPaint.ImageFilter = new DropShadow(0, 0, 3, 3, SKColors.Black.WithAlpha(255 / 4));

        return settings.WithTooltipBackgroundPaint(backgroundPaint);
    }

    public static LiveChartsSettings WithLegendPosition(this LiveChartsSettings settings, LegendPosition position)
    {
        settings.LegendPosition = position;
        return settings;
    }
    
    public static LiveChartsSettings WithTooltipPosition(this LiveChartsSettings settings, TooltipPosition position)
    {
        settings.TooltipPosition = position;
        return settings;
    }
    
    public static LiveChartsSettings AddDefaultTheme
    (
        this LiveChartsSettings settings,
        LvcTheme theme,
        Action<Theme<SkiaSharpDrawingContext>>? additionalStyles = null)
    {
        if (theme is LvcTheme.Dark)
            settings.AddDarkTheme(additionalStyles);
        else
            settings.AddLightTheme(additionalStyles);

        return settings;
    }
    
    public static LiveChartsSettings Dispose(this LiveChartsSettings settings) => settings
        .DisposeDefaultTypeface()
        .DisposeLegendAndTooltipPaints();

    public static LiveChartsSettings DisposeDefaultTypeface(this LiveChartsSettings settings)
    {
        LiveChartsSkiaSharp.DefaultSKTypeface?.Dispose();
        return settings;
    }

    public static LiveChartsSettings DisposeLegendAndTooltipPaints(this LiveChartsSettings settings)
    {
        settings.GetLegendTextPaint()?.Dispose();
        settings.GetLegendBackgroundPaint()?.Dispose();
        settings.GetTooltipTextPaint()?.Dispose();
        settings.GetTooltipBackgroundPaint()?.Dispose();
        return settings;
    }
    
    public static Paint? GetLegendTextPaint(this LiveChartsSettings settings) =>
        settings.LegendTextPaint as Paint;
    
    public static Paint? GetLegendBackgroundPaint(this LiveChartsSettings settings) =>
        settings.LegendBackgroundPaint as Paint;

    public static Paint? GetTooltipTextPaint(this LiveChartsSettings settings) =>
        settings.TooltipTextPaint as Paint;
    
    public static Paint? GetTooltipBackgroundPaint(this LiveChartsSettings settings) =>
        settings.TooltipBackgroundPaint as Paint;
}