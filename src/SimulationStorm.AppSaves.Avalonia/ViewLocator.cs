using SimulationStorm.AppSaves.Avalonia.Views;
using SimulationStorm.AppSaves.Presentation.ViewModels;
using SimulationStorm.Avalonia;

namespace SimulationStorm.AppSaves.Avalonia;

public class ViewLocator : StrongViewLocatorBase
{
    public ViewLocator()
    {
        Register<AppSaveManagerDialogViewModel, AppSaveManagerDialogView>();
        Register<EditAppSaveFlyoutViewModel, EditAppSaveFlyoutView>();
        Register<SaveAppFlyoutViewModel, SaveAppFlyoutView>();
        Register<DeleteAllAppSavesFlyoutViewModel, DeleteAllAppSavesFlyoutView>();
    }
}