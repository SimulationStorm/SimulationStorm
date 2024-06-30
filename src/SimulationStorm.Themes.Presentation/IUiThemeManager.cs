using System;

namespace SimulationStorm.Themes.Presentation;

/// <summary>
/// Provides a mechanism to change a user interface theme.
/// </summary>
public interface IUiThemeManager
{
    /// <summary>
    /// Gets the current user interface theme.
    /// </summary>
    UiTheme CurrentTheme { get; }

    /// <summary>
    /// Occurs when the user interface theme changes.
    /// </summary>
    event EventHandler? ThemeChanged;

    /// <summary>
    /// Changes the current user interface theme to <see cref="newTheme"/>.
    /// </summary>
    /// <param name="newTheme">The new user interface theme.</param>
    void ChangeTheme(UiTheme newTheme);

    /// <summary>
    /// Switches the current user interface theme to the opposite one.
    /// </summary>
    void ToggleTheme();
}