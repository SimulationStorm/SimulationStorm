using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using GenericCellularAutomation.Presentation.CellStates;

namespace GenericCellularAutomation.Avalonia.Views;

public partial class RuleView : UserControl
{
    public static readonly StyledProperty<ObservableCollection<CellStateModel>> CellStateModelsProperty =
        RuleSetView.CellStateModelsProperty.AddOwner<RuleView>();

    public ObservableCollection<CellStateModel> CellStateModels
    {
        get => GetValue(CellStateModelsProperty);
        set => SetValue(CellStateModelsProperty, value);
    }
    
    public RuleView() => InitializeComponent();
}