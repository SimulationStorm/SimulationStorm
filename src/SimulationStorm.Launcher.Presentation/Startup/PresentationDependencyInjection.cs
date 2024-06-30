using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.Launcher.Presentation.ViewModels;
using SimulationStorm.Presentation;
using SimulationStorm.Presentation.Navigation;

namespace SimulationStorm.Launcher.Presentation.Startup;

public static class PresentationDependencyInjection
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services) => services
        .AddSingleton<IShutdownService>(sp =>
        {
            var navigationService = sp.GetRequiredService<INavigationManager>();
            return new ShutdownService(() => navigationService.NavigateToPreviousContent(), isImmediateShutdownSupported: false);
        })
        .AddSingleton<MainViewModel>();
}