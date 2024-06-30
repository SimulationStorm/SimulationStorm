using System;
using CommunityToolkit.Mvvm.Input;

namespace SimulationStorm.Flyouts.Presentation;

public interface IFlyoutViewModel
{
    bool IsBackgroundClosingAllowed { get; }
    
    IRelayCommand CloseCommand { get; }
    
    event EventHandler? CloseRequested;

    void OnClosing();
}