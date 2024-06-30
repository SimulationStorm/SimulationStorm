using System;

namespace SimulationStorm.Dialogs.Presentation;

public interface IDialog
{
    bool IsBackgroundClosingAllowed { get; }
    
    event EventHandler? CloseRequested;
}