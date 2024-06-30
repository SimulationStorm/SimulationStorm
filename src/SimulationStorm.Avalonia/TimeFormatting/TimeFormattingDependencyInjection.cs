using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.Presentation.TimeFormatting;

namespace SimulationStorm.Avalonia.TimeFormatting;

public static class TimeFormattingDependencyInjection
{
    public static IServiceCollection AddTimeFormattingServices(this IServiceCollection services) => services
        .AddSingleton(TimeFormattingConfiguration.TimeFormatterOptions)
        .AddSingleton<ITimeFormatter, TimeFormatter>();
}