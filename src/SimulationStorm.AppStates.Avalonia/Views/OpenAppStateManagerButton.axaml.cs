using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.AppStates.Presentation.ViewModels;
using SimulationStorm.DependencyInjection;
using SimulationStorm.Dialogs.Presentation;

namespace SimulationStorm.AppStates.Avalonia.Views;

public partial class OpenAppStateManagerButton : Button
{
    public OpenAppStateManagerButton()
    {
        InitializeComponent();

        var dialogManager = DiContainer.Default.GetRequiredService<IDialogManager>();

        Command = new RelayCommand(() =>
        {
            var appStateManagerDialogViewModel = DiContainer.Default.GetRequiredService<AppStateManagerDialogViewModel>();
            dialogManager.ShowDialog(appStateManagerDialogViewModel);
        });
    }
}