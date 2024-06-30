using Avalonia;
using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.DependencyInjection;

namespace SimulationStorm.Avalonia.Extensions;

public static class StyledElementExtensions
{
    public static void ResolveViewModelFromDefaultDiContainer<TViewModel>(this StyledElement styledElement)
        where TViewModel : class => styledElement.DataContext = DiContainer.Default.GetRequiredService<TViewModel>();
}