using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using DotNext.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.Avalonia.Controls;
using SimulationStorm.Avalonia.Extensions;
using SimulationStorm.DependencyInjection;
using SimulationStorm.Dialogs.Presentation;
using SimulationStorm.Exceptions;
using SimulationStorm.Exceptions.Avalonia;
using SimulationStorm.Exceptions.Logging;
using SimulationStorm.GameOfLife.Avalonia.Views;
using SimulationStorm.GameOfLife.Presentation.Startup;
using SimulationStorm.Localization.Avalonia;
using SimulationStorm.Logging;
using SimulationStorm.Notifications.Avalonia;
using SimulationStorm.Presentation;
using SimulationStorm.Presentation.StartupOperations;
using SimulationStorm.Simulation.CellularAutomation.Avalonia.Views;

namespace SimulationStorm.GameOfLife.Avalonia.Startup;

public partial class LoaderView : UserControl
{
    #region Fields
    private readonly IShutdownService _shutdownService = null!;

    private IServiceCollection _essentialServices = null!;
    
    private IDiContainer _essentialServicesContainer = null!;
    #endregion

    public LoaderView() { }
    
    public LoaderView(IShutdownService shutdownService)
    {
        _shutdownService = shutdownService;
        
        InitializeComponent();
        InitializeEssentialServices();
        CreateAndShowWarningScreen();
    }

#if DEBUG
    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        _ = StartLoadingAsync();
    }
#endif

    #region Private methods
    private void InitializeEssentialServices()
    {
        _essentialServices = new ServiceCollection()
            .AddLocalizationManager(Resources, AvaloniaConfiguration.LocalizationOptions)
            .AddSingleton<IDialogManager>(DialogManager)
            .AddNotificationManagers(NotificationManager, Resources, AvaloniaConfiguration.NotificationsOptions)
            // Exception services
            // .AddExceptionHandlersToNotifiersSubscriber()
            // Exception notifiers
            // .AddAppDomainExceptionNotifier()
            // .AddTaskSchedulerExceptionNotifier()
            // .AddDispatcherExceptionNotifier()
            // Exception handlers
            // .AddUnhandledExceptionSuppressor()
            //
            // Logging services
            // .AddLoggingServices(PresentationConfiguration.LoggingOptions)
            // .AddUnhandledExceptionLogger()
            //
            ;
            
        _essentialServicesContainer = new DiContainer();
        _essentialServicesContainer.Configure(_essentialServices, activateSingletonServices: true);
    }

    private void CreateAndShowWarningScreen()
    {
        var warningScreen = new WarningScreen();
        ContentControl.Content = warningScreen;
        
#if DEBUG
        return;
#endif
        
        this.WithDisposables(disposables =>
        {
            Observable
                .FromEventPattern<EventHandler<RoutedEventArgs>, RoutedEventArgs>
                (
                    h => warningScreen.QuitRequested += h,
                    h => warningScreen.QuitRequested -= h
                )
                .Take(1)
                .Subscribe(_ =>
                {
                    if (_shutdownService.IsShutdownSupported)
                        _shutdownService.Shutdown();
                    else
                        ContentControl.Content = null; // In case when it is a single view application, just show empty view
                })
                .DisposeWith(disposables);
            
            Observable
                .FromEventPattern<EventHandler<RoutedEventArgs>, RoutedEventArgs>
                (
                    h => warningScreen.ContinueRequested += h,
                    h => warningScreen.ContinueRequested -= h
                )
                .Take(1)
                .Subscribe(e => _ = StartLoadingAsync())
                .DisposeWith(disposables);
        });
    }
    
    private async Task StartLoadingAsync()
    {
        var loadingScreen = new LoadingExitingScreen
        {
            Logotype = new Logotype()
        };
        ContentControl.Content = loadingScreen;
        await WaitForContentTransitionCompletingAsync();
        await loadingScreen.ShowProgressAsync();
        
        await Task.Run(() =>
        {
            SetupMainServiceCollectionAndDefaultDiContainer();

            var startupOperationManager = DiContainer.Default.GetRequiredService<IStartupOperationManager>();
            startupOperationManager.ExecuteStartupOperations();
        });

        await loadingScreen.HideProgressAsync();
        var mainView = new MainView(ShutdownImmediatelyOrStartUnloadingAsync);
        ContentControl.Content = mainView;
    }

    private void SetupMainServiceCollectionAndDefaultDiContainer()
    {
        var mainServices = new ServiceCollection()
            .AddAvaloniaServices()
            .AddPresentationServices();
        
        AddEssentialServiceInstancesToMainServiceCollection(mainServices);
            
        DiContainer.Default.Configure(mainServices, activateSingletonServices: true);
    }

    private void AddEssentialServiceInstancesToMainServiceCollection(IServiceCollection mainServices)
    {
        _essentialServices.ForEach(serviceDescriptor =>
        {
            var serviceType = serviceDescriptor.ServiceType;
            // If it is ILogger<> like service, add descriptor directly
            if (serviceType.IsGenericTypeDefinition)
            {
                mainServices.Add(serviceDescriptor);
                return;
            }
            
            var serviceInstance = _essentialServicesContainer.GetService(serviceType)!;
            mainServices.AddSingleton(serviceType, serviceInstance);
        });
    }

    private void ShutdownImmediatelyOrStartUnloadingAsync()
    {
        if (_shutdownService.IsImmediateShutdownSupported)
            _shutdownService.Shutdown();
        else
            _ = StartUnloadingAsync();
    }

    private async Task StartUnloadingAsync()
    {
        var exitingScreen = new LoadingExitingScreen
        {
            Logotype = new Logotype(),
            IsExiting = true
        };
        ContentControl.Content = exitingScreen;
        await WaitForContentTransitionCompletingAsync();
        await exitingScreen.ShowProgressAsync();
        
        await DiContainer.Default.DisposeAsync();
        
        await exitingScreen.HideProgressAsync();
        _shutdownService.Shutdown();
    }

    private Task WaitForContentTransitionCompletingAsync() => Task.Delay(ContentControl.GetTransitionDuration());
    #endregion
}