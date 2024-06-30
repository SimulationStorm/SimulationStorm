using SimulationStorm.AppStates.Avalonia.Views;
using SimulationStorm.AppStates.Presentation.ViewModels;
using SimulationStorm.Avalonia;

namespace SimulationStorm.AppStates.Avalonia;

public class ViewLocator : StrongViewLocatorBase
{
    public ViewLocator()
    {
        Register<AppStateManagerDialogViewModel, AppStateManagerDialogView>();
        Register<EditAppStateFlyoutViewModel, EditAppStateFlyoutView>();
        Register<SaveAppStateFlyoutViewModel, SaveAppStateFlyoutView>();
        Register<DeleteAllAppStatesFlyoutViewModel, DeleteAllAppStatesFlyoutView>();
    }
}