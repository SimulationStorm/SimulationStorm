using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimulationStorm.Densities.Presentation;
using SimulationStorm.Launcher.Presentation.Models;
using SimulationStorm.Launcher.Presentation.Services;
using SimulationStorm.Localization.Presentation;
using SimulationStorm.Notifications.Presentation;
using SimulationStorm.Presentation;
using SimulationStorm.Presentation.Navigation;
using SimulationStorm.Themes.Presentation;
using SimulationStorm.Utilities.Disposing;

namespace SimulationStorm.Launcher.Presentation.ViewModels;

public partial class MainViewModel : DisposableObservableObject
{
    #region Properties
    public CultureInfo CurrentCulture
    {
        get => _localizationManager.CurrentCulture;
        set => _localizationManager.ChangeCulture(value);
    }

    public IEnumerable<CultureInfo> Cultures => _localizationManager.SupportedCultures;

    public UiTheme CurrentTheme
    {
        get => _uiThemeManager.CurrentTheme;
        set => _uiThemeManager.ChangeTheme(value);
    }

    public IEnumerable<UiTheme> Themes { get; } = Enum.GetValues<UiTheme>();
    
    public UiDensity CurrentDensity
    {
        get => _uiDensityManager.CurrentDensity;
        set => _uiDensityManager.ChangeDensity(value);
    }

    public IEnumerable<UiDensity> Densities { get; } = new[] { UiDensity.Normal, UiDensity.Spacious };

    [ObservableProperty] private SimulationType _selectedSimulationType;

    public IEnumerable<SimulationType> SimulationTypes { get; } = Enum.GetValues<SimulationType>();
    #endregion

    #region Fields
    private readonly ILocalizationManager _localizationManager;
    
    private readonly IUiThemeManager _uiThemeManager;

    private readonly IUiDensityManager _uiDensityManager;
    
    private readonly INavigationManager _navigationManager;

    private readonly ISimulationLoaderViewFactory _simulationLoaderViewFactory;

    private readonly ISimulationApplicationNameProvider _simulationApplicationNameProvider;
    
    private readonly ILocalizedNotificationManager _notificationManager;

    private bool _skipNavigationContentChanged;

    private IDisposable? _simulationProcessLock;
    #endregion

    public MainViewModel
    (
        ILocalizationManager localizationManager,
        IUiThemeManager uiThemeManager,
        IUiDensityManager uiDensityManager,
        INavigationManager navigationManager,
        ISimulationLoaderViewFactory simulationLoaderViewFactory,
        ISimulationApplicationNameProvider simulationApplicationNameProvider,
        ILocalizedNotificationManager notificationManager)
    {
        _localizationManager = localizationManager;
        _uiThemeManager = uiThemeManager;
        _uiDensityManager = uiDensityManager;
        _navigationManager = navigationManager;
        _simulationLoaderViewFactory = simulationLoaderViewFactory;
        _simulationApplicationNameProvider = simulationApplicationNameProvider;
        _notificationManager = notificationManager;
        
        Observable
            .FromEventPattern<EventHandler<CultureChangedEventArgs>, CultureChangedEventArgs>
            (
                h => _localizationManager.CultureChanged += h,
                h => _localizationManager.CultureChanged -= h
            )
            .Subscribe(_ => OnPropertyChanged(nameof(CurrentCulture)))
            .DisposeWith(Disposables);
        
        Observable
            .FromEventPattern<EventHandler, EventArgs>
            (
                h => _uiThemeManager.ThemeChanged += h,
                h => _uiThemeManager.ThemeChanged -= h
            )
            .Subscribe(_ => OnPropertyChanged(nameof(CurrentTheme)))
            .DisposeWith(Disposables);
        
        Observable
            .FromEventPattern<EventHandler, EventArgs>
            (
                h => _uiDensityManager.DensityChanged += h,
                h => _uiDensityManager.DensityChanged -= h
            )
            .Subscribe(_ => OnPropertyChanged(nameof(CurrentDensity)))
            .DisposeWith(Disposables);
        
        Observable
            .FromEventPattern<EventHandler<NavigationContentChangedEventArgs>, NavigationContentChangedEventArgs>
            (
                h => _navigationManager.ContentChanged += h,
                h => _navigationManager.ContentChanged -= h
            )
            .Subscribe(_ =>
            {
                if (_skipNavigationContentChanged)
                {
                    _skipNavigationContentChanged = false;
                    return;
                }
                
                // ? is used here instead of ! to avoid error when initial navigation to main view
                _simulationProcessLock?.Dispose();
            })
            .DisposeWith(Disposables);
    }
    
    [RelayCommand]
    private void LaunchSimulation()
    {
        var simulationApplicationName = _simulationApplicationNameProvider.GetApplicationName(SelectedSimulationType);
        if (!ApplicationProcessManager.TryGetLock(simulationApplicationName, out _simulationProcessLock))
        {
            _notificationManager.ShowError("Launcher.SelectedSimulationIsAlreadyRunning", "Notifications.Error");
            return;
        }
        
        var simulationLoaderView = _simulationLoaderViewFactory.Create(SelectedSimulationType);
        
        _skipNavigationContentChanged = true;
        _navigationManager.Navigate(simulationLoaderView);
    }
}