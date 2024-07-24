using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using GenericCellularAutomation.Presentation.CellStates;

namespace GenericCellularAutomation.Avalonia.Views;

public partial class RuleSetView : UserControl
{
    public static readonly StyledProperty<ObservableCollection<CellStateModel>> CellStateModelsProperty =
        AvaloniaProperty.Register<RuleView, ObservableCollection<CellStateModel>>(nameof(CellStateModels));

    public ObservableCollection<CellStateModel> CellStateModels
    {
        get => GetValue(CellStateModelsProperty);
        set => SetValue(CellStateModelsProperty, value);
    }
    
    public RuleSetView() => InitializeComponent();
}