using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ActiproSoftware.UI.Avalonia.Themes;
using ActiproSoftware.UI.Avalonia.Themes.Generation;
using Avalonia.Threading;
using DynamicData.Binding;
using SimulationStorm.Avalonia.Extensions;
using SimulationStorm.Densities.Presentation;
using SimulationStorm.Utilities.Disposing;

namespace SimulationStorm.Densities.Avalonia;

/// <inheritdoc cref="IUiDensityManager"/>
public class UiDensityManager : DisposableObject, IUiDensityManager
{
    /// <inheritdoc/>
    public UiDensity CurrentDensity => _modernTheme
        .GetValueOnUiThread(ModernTheme.DefinitionProperty)!.UserInterfaceDensity.ToUiDensity();

    /// <inheritdoc/>
    public event EventHandler? DensityChanged;

    #region Fields
    private readonly ModernTheme _modernTheme;

    private bool _skipModernThemeDensityChangedNotification;
    #endregion

    public UiDensityManager(ModernTheme modernTheme)
    {
        _modernTheme = modernTheme;

        WithDisposables(disposables =>
        {
            IDisposable? definitionUiDensitySubscription = null;
            
            _modernTheme
                .GetObservableOnUiThread(ModernTheme.DefinitionProperty)
                .Where(definition => definition is not null)
                .Select(definition => definition!)
                .Subscribe(definition =>
                {
                    definitionUiDensitySubscription?.Dispose();
                    
                    definitionUiDensitySubscription =
                        SubscribeOnThemeDefinitionUiDensityChange(definition);
                    
                    NotifyDensityChanged();
                })
                .DisposeWith(disposables);
            
            disposables.Add(Disposable.Create(() => definitionUiDensitySubscription?.Dispose()));
        });

        return;
        
        IDisposable SubscribeOnThemeDefinitionUiDensityChange(ThemeDefinition definition) => definition
            .WhenValueChanged(x => x.UserInterfaceDensity, false)
            .Where(_ => !_skipModernThemeDensityChangedNotification)
            .Subscribe(_ => NotifyDensityChanged());
    }

    /// <inheritdoc/>
    public void ChangeDensity(UiDensity newDensity)
    {
        var previousDensity = CurrentDensity;
        if (newDensity == previousDensity)
            return;
        
        var newActiproDensity = newDensity.ToActipro();

        var themeDefinition = _modernTheme.Definition!;
        Dispatcher.UIThread.Post(() =>
        {
            // Optionally update the base font size based on the density
            themeDefinition.BaseFontSize = newActiproDensity switch
            {
                UserInterfaceDensity.Compact => 13,
                _ => 14
            };
            // Set the new UI density
            themeDefinition.UserInterfaceDensity = newActiproDensity;
            // Must manually refresh resources after changing definition properties

            _skipModernThemeDensityChangedNotification = true;
            _modernTheme.RefreshResources();
            _skipModernThemeDensityChangedNotification = false;
        
            NotifyDensityChanged();
        });
    }
    
    private void NotifyDensityChanged() => DensityChanged?.Invoke(this, EventArgs.Empty);
}