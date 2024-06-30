using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Controls.Notifications;
using SimulationStorm.Avalonia.Controls;
using SimulationStorm.Avalonia.Extensions;
using SimulationStorm.Launcher.Presentation.ViewModels;
using SimulationStorm.Presentation.Navigation;

namespace SimulationStorm.Launcher.Avalonia.Startup;

public partial class LoaderWindow : WindowExtended
{
    public INavigationManager NavigationManager => InternalNavigationManager;

    public WindowNotificationManager WindowNotificationManager => InternalNotificationManager;
    
    public LoaderWindow()
    {
        InitializeComponent();

        this.WithDisposables(disposables =>
        {
            // Hide title when current content is not MainViewModel
            Observable
                .FromEventPattern<EventHandler<NavigationContentChangedEventArgs>, NavigationContentChangedEventArgs>
                (
                    h => NavigationManager.ContentChanged += h,
                    h => NavigationManager.ContentChanged -= h
                )
                .Select(e => e.EventArgs.NewContent)
                .Subscribe(currentContent => IsTitleBarVisible = currentContent is MainViewModel)
                .DisposeWith(disposables);
        });
    }
}