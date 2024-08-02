using CommunityToolkit.Mvvm.ComponentModel;
using GenericCellularAutomation.Presentation.CellStates.Descriptors;

namespace GenericCellularAutomation.Presentation;

public sealed partial class GcaSettings(GcaOptions gcaOptions) : ObservableObject
{
    [ObservableProperty] private CellStateCollectionDescriptor _cellStateCollectionDescriptor =
        gcaOptions.Configuration.CellStateCollection;
}