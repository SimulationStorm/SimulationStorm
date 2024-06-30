using System;
using System.Collections.Generic;
using System.Globalization;

namespace SimulationStorm.Localization.Presentation;

/// <summary>
/// Provides a mechanism to localize the application.
/// </summary>
public interface ILocalizationManager
{
    #region Properties
    /// <summary>
    /// Gets cultures for which localization is available.
    /// </summary>
    IEnumerable<CultureInfo> SupportedCultures { get; }

    /// <summary>
    /// Gets the culture for which the application is localized for.
    /// </summary>
    CultureInfo CurrentCulture { get; }
    #endregion

    event EventHandler<CultureChangedEventArgs>? CultureChanged;

    #region Methods
    /// <summary>
    /// Changes the culture for which the application needs to be localized.
    /// </summary>
    /// <param name="newCulture">The culture to localize application for.</param>
    /// <exception cref="System.NotSupportedException">The culture is not supported.</exception>
    void ChangeCulture(CultureInfo newCulture);
    
    /// <summary>
    /// Gets the localized string by the given <see cref="key"/>.
    /// </summary>
    /// <param name="key">The localized string key.</param>
    /// <returns>The localized string, or null if there is no localized string by the given <see cref="key"/>.</returns>
    /// <exception cref="InvalidOperationException">The localized string with the given key was not found.</exception>
    string GetLocalizedString(string key);
    #endregion
}