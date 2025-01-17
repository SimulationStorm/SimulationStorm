﻿using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.AppSaves.Presentation;
using SimulationStorm.Avalonia;
using SimulationStorm.Themes.Presentation;

namespace SimulationStorm.Themes.Avalonia;

public static class UiThemesDependencyInjection
{
    public static IServiceCollection AddUiThemeManager(this IServiceCollection services) => services
        .AddSingleton<IUiThemeManager>(_ => new UiThemeManager(AvaloniaServiceProvider.GetApplicationOrThrow()))
        .AddServiceSaveManager<UiThemeSaveManager>();
}