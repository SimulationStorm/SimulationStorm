using System;
using Avalonia.Styling;
using SimulationStorm.Themes.Presentation;

namespace SimulationStorm.Themes.Avalonia;

public static class UiThemeExtensions
{
    public static ThemeVariant ToAvalonia(this UiTheme uiTheme) => uiTheme switch
    {
        UiTheme.Dark => ThemeVariant.Dark,
        UiTheme.Light => ThemeVariant.Light,
        _ => throw new ArgumentException("Invalid UI theme.", nameof(uiTheme))
    };

    public static UiTheme ToUiTheme(this ThemeVariant themeVariant)
    {
        if (themeVariant == ThemeVariant.Dark)
            return UiTheme.Dark;
        
        if (themeVariant == ThemeVariant.Light)
            return UiTheme.Light;

        throw new ArgumentException("Invalid theme variant", nameof(themeVariant));
    }
}