using CommunityToolkit.Mvvm.ComponentModel;
using GenericCellularAutomation.Presentation.Common;
using SimulationStorm.Graphics;

namespace GenericCellularAutomation.Presentation.CellStates;

public sealed partial class CellStateModel : NamedIndexedObservableObject
{
    public byte CellState { get; init; }

    [ObservableProperty] private bool _isDefault;

    [ObservableProperty] private Color _color;
}