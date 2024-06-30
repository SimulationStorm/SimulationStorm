using CommunityToolkit.Mvvm.Input;

namespace SimulationStorm.Simulation.CellularAutomation.Presentation.ViewModels;

public interface IWorldWrappingViewModel
{
    WorldWrapping ActualWorldWrapping { get; }
    
    bool IsWrappedHorizontally { get; set; }
    
    bool IsWrappedVertically { get; set; }
    
    IAsyncRelayCommand ChangeWorldWrappingCommand { get; }
}