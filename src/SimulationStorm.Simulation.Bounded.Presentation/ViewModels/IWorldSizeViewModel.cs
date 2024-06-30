using CommunityToolkit.Mvvm.Input;
using SimulationStorm.Primitives;

namespace SimulationStorm.Simulation.Bounded.Presentation.ViewModels;

public interface IWorldSizeViewModel
{
    #region Properties
    Range<Size> WorldSizeRange { get; }
    
    Size ActualWorldSize { get; }
    
    int EditingWorldWidth { get; set; }
    
    int EditingWorldHeight { get; set; }
    
    bool KeepAspectRatio { get; set; }
    #endregion
    
    IAsyncRelayCommand ChangeWorldSizeCommand { get; }
}