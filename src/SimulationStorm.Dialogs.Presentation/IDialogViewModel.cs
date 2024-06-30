using System;
using CommunityToolkit.Mvvm.Input;

namespace SimulationStorm.Dialogs.Presentation;

public interface IDialogViewModel
{
    bool IsBackgroundClosingAllowed { get; }
    
    IRelayCommand CloseCommand { get; }
    
    event EventHandler? CloseRequested;
}