using CommunityToolkit.Mvvm.ComponentModel;
using GenericCellularAutomation.Presentation.CellStates.Descriptors;

namespace GenericCellularAutomation.Presentation;

public sealed partial class GenericCellularAutomationSettings : ObservableObject
{
    // Todo: initialize this
    [ObservableProperty] private CellStateCollectionDescriptor _cellStateCollectionDescriptor = null!;
}