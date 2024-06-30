using System.Collections.Generic;
using System.Globalization;
using Avalonia.Markup.Xaml.Styling;

namespace SimulationStorm.Localization.Avalonia;

/// <summary>
/// Provides locale resource collection by <see cref="CultureInfo"/>.
/// </summary>
public interface ILocaleResourceProvider
{
    /// <summary>
    /// Gets the cultures for which there are locale resources.
    /// </summary>
    IEnumerable<CultureInfo> AvailableCultures { get; }
    
    /// <summary>
    /// Gets locale resources by the <see cref="culture"/>.
    /// </summary>
    /// <param name="culture">The culture for which you need to obtain resources.</param>
    /// <returns>Locale resource collection.</returns>
    IEnumerable<ResourceInclude> GetLocaleResourcesByCulture(CultureInfo culture);
}