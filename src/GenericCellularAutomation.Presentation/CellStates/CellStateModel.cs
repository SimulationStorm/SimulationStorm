using CommunityToolkit.Mvvm.ComponentModel;
using SimulationStorm.Graphics;

namespace GenericCellularAutomation.Presentation.CellStates;

public partial class CellStateModel : ObservableObject
{
    public byte CellState { get; init; }

    [ObservableProperty] private bool _isDefault;
    
    [ObservableProperty] private string _name;

    [ObservableProperty] private Color _color;
}