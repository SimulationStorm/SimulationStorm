using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.Avalonia;
using SimulationStorm.DependencyInjection;
using SimulationStorm.Launcher.Presentation.Startup;
using SimulationStorm.Launcher.Presentation.ViewModels;
using SimulationStorm.Notifications.Avalonia;
using SimulationStorm.Presentation.Navigation;

namespace SimulationStorm.Launcher.Avalonia.Startup;

public class App : DesktopApplication
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    protected override void InitializeApplication(IClassicDesktopStyleApplicationLifetime desktop)
    {
        BindingPlugins.DataValidators.RemoveAt(0);
        
        var services = new ServiceCollection()
            .AddAvaloniaServices(Resources)
            .AddPresentationServices();

        var loaderWindow = new LoaderWindow();
        services.AddSingleton(loaderWindow.NavigationManager);
        services.AddNotificationManagers(loaderWindow.WindowNotificationManager,
            Resources, AvaloniaConfiguration.NotificationsOptions);

        var diContainer = new DiContainer();
        diContainer.Configure(services, activateSingletonServices: true);
        
        var mainViewModel = diContainer.GetRequiredService<MainViewModel>();
        var navigationManager = diContainer.GetRequiredService<INavigationManager>();
        navigationManager.Navigate(mainViewModel);
        
        desktop.MainWindow = loaderWindow;
    }
}