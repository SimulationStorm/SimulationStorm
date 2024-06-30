using System;
using System.Globalization;

namespace SimulationStorm.Localization.Presentation;

/// <summary>
/// Provides the ability to change the all four culture properties of the <see cref="CultureInfo"/> class
/// and ability to observe culture changes.
/// </summary>
/// <remarks>
/// Currently used to synchronize culture changes in simulation and launcher.
/// Could be eliminated after separate process simulation running implementation.
/// </remarks>
public class CultureManager
{
    public static readonly CultureManager Instance = new();

    public CultureInfo CurrentCulture => CultureInfo.CurrentCulture;
    
    public event EventHandler? CultureChanged;
    
    private CultureManager() { }
    
    public void ChangeCulture(CultureInfo newCulture)
    {
        var previousCulture = CurrentCulture;
        if (Equals(newCulture, previousCulture))
            return;
        
        SetCulture(newCulture);
        NotifyCultureChanged();
    }

    private static void SetCulture(CultureInfo culture)
    {
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
    }
    
    private void NotifyCultureChanged() => CultureChanged?.Invoke(this, EventArgs.Empty);
}