using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using GenericCellularAutomation.Presentation.CellStates;

namespace GenericCellularAutomation.Avalonia.Views;

public partial class CellStateView : UserControl
{
    #region Avalonia properties
    public static readonly StyledProperty<CellStateModel?> CellStateModelProperty =
        AvaloniaProperty.Register<CellStateView, CellStateModel?>(nameof(CellStateModel));

    public static readonly StyledProperty<int> MaxNameLengthProperty =
        AvaloniaProperty.Register<CellStateView, int>(nameof(MaxNameLength));

    public static readonly StyledProperty<ICommand?> DeleteCommandProperty =
        AvaloniaProperty.Register<CellStateView, ICommand?>(nameof(DeleteCommand));

    public static readonly StyledProperty<ICommand?> MarkAsDefaultCommandProperty =
        AvaloniaProperty.Register<CellStateView, ICommand?>(nameof(MarkAsDefaultCommand));
    #endregion

    #region Properties
    public CellStateModel? CellStateModel
    {
        get => GetValue(CellStateModelProperty);
        set => SetValue(CellStateModelProperty, value);
    }
    
    public int MaxNameLength
    {
        get => GetValue(MaxNameLengthProperty);
        set => SetValue(MaxNameLengthProperty, value);
    }
    
    public ICommand? DeleteCommand
    {
        get => GetValue(DeleteCommandProperty);
        set => SetValue(DeleteCommandProperty, value);
    }

    public ICommand? MarkAsDefaultCommand
    {
        get => GetValue(MarkAsDefaultCommandProperty);
        set => SetValue(MarkAsDefaultCommandProperty, value);
    }
    #endregion
    
    public CellStateView() => InitializeComponent();
}