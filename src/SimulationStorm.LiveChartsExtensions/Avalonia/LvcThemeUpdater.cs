using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using SimulationStorm.LiveChartsExtensions.ThemeManagement;
using SimulationStorm.Themes.Presentation;
using SimulationStorm.Utilities.Disposing;

namespace SimulationStorm.LiveChartsExtensions.Avalonia;

public class LvcThemeUpdater : DisposableObject
{
    private readonly IUiThemeManager _uiThemeManager;
    
    public LvcThemeUpdater(IUiThemeManager uiThemeManager)
    {
        _uiThemeManager = uiThemeManager;
        
        WithDisposables(disposables =>
        {
            Observable
                .FromEventPattern<EventHandler, EventArgs>
                (
                    h => uiThemeManager.ThemeChanged += h,
                    h => uiThemeManager.ThemeChanged -= h
                )
                .Subscribe(_ => UpdateLiveChartsTheme())
                .DisposeWith(disposables);
        });
        
        UpdateLiveChartsTheme();
    }

    private void UpdateLiveChartsTheme()
    {
        if (_uiThemeManager.CurrentTheme == UiTheme.Dark)
            LvcThemeManager.Instance.ChangeTheme(LvcTheme.Dark);
        else
            LvcThemeManager.Instance.ChangeTheme(LvcTheme.Light);
    }
}