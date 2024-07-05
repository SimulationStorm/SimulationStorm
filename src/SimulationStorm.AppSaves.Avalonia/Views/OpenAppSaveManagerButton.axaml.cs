using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.AppSaves.Presentation.ViewModels;
using SimulationStorm.DependencyInjection;
using SimulationStorm.Dialogs.Presentation;

namespace SimulationStorm.AppSaves.Avalonia.Views;

public partial class OpenAppSaveManagerButton : Button
{
    public OpenAppSaveManagerButton()
    {
        InitializeComponent();

        var dialogManager = DiContainer.Default.GetRequiredService<IDialogManager>();

        Command = new RelayCommand(() =>
        {
            var appSaveManagerDialogViewModel = DiContainer.Default.GetRequiredService<AppSaveManagerDialogViewModel>();
            dialogManager.ShowDialog(appSaveManagerDialogViewModel);
        });
    }
}