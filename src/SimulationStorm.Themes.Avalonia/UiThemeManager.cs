using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Threading;
using SimulationStorm.Avalonia.Extensions;
using SimulationStorm.Themes.Presentation;
using SimulationStorm.Utilities.Disposing;

namespace SimulationStorm.Themes.Avalonia;

/// <inheritdoc cref="IUiThemeManager"/>
public class UiThemeManager : DisposableObject, IUiThemeManager
{
    /// <inheritdoc/>
    public UiTheme CurrentTheme =>
        _application.GetValueOnUiThread(Application.ActualThemeVariantProperty).ToUiTheme();

    /// <inheritdoc/>
    public event EventHandler? ThemeChanged;

    #region Fields
    private readonly Application _application;

    private bool _skipApplicationThemeChangedNotification;
    #endregion

    public UiThemeManager(Application application)
    {
        _application = application;
        
        Observable
            .FromEventPattern<EventHandler, EventArgs>
            (
                h => _application.ActualThemeVariantChanged += h,
                h => _application.ActualThemeVariantChanged -= h
            )
            .Where(_ => !_skipApplicationThemeChangedNotification)
            .Select(e => e.EventArgs)
            .Subscribe(_ => NotifyThemeChanged())
            .DisposeWith(Disposables);
    }
    
    #region Public methods
    /// <inheritdoc/>
    public void ChangeTheme(UiTheme newTheme)
    {
        if (newTheme == CurrentTheme)
            return;
        
        Dispatcher.UIThread.Post(() =>
        {
            _skipApplicationThemeChangedNotification = true;
            _application.RequestedThemeVariant = newTheme.ToAvalonia();
            _skipApplicationThemeChangedNotification = false;

            NotifyThemeChanged();
        });
    }

    /// <inheritdoc/>
    public void ToggleTheme() => ChangeTheme(CurrentTheme is UiTheme.Dark ? UiTheme.Light : UiTheme.Dark);
    #endregion
    
    private void NotifyThemeChanged() => ThemeChanged?.Invoke(this, EventArgs.Empty);
}