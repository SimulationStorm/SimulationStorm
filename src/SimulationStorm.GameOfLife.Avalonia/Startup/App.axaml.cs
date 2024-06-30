using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.Avalonia;
using SimulationStorm.DependencyInjection;
using SimulationStorm.Dialogs.Avalonia;
using SimulationStorm.Localization.Avalonia;
using SimulationStorm.Localization.Presentation;
using SimulationStorm.Presentation;

namespace SimulationStorm.GameOfLife.Avalonia.Startup;

public class App : SingleProcessApplication
{
    public App() => Name = AvaloniaConfiguration.ApplicationName;
    
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    protected override void InitializeApplication(IClassicDesktopStyleApplicationLifetime desktop)
    {
        BindingPlugins.DataValidators.RemoveAt(0);
        
        var shutdownService = new ShutdownService(() => desktop.Shutdown(), isImmediateShutdownSupported: true);
        desktop.MainWindow = new LoaderWindow(shutdownService);
    }

    protected override void OnApplicationIsAlreadyRunning(IClassicDesktopStyleApplicationLifetime desktop)
    {
        // The following code is needed to set up localization manager
        var services = new ServiceCollection()
            .AddLocalizationManager(Resources, AvaloniaConfiguration.LocalizationOptions);

        var diContainer = new DiContainer();
        diContainer.Configure(services, activateSingletonServices: true);

        var localizationManager = diContainer.GetRequiredService<ILocalizationManager>();
        
        desktop.MainWindow = new MessageBox
        {
            Title = AvaloniaConfiguration.ApplicationName,
            Icon = AvaloniaConfiguration.ApplicationIcon,
            Message = localizationManager.GetLocalizedString("GameOfLife.AlreadyRunning")
        };
    }
}