using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using DynamicData.Binding;
using GenericCellularAutomation.Presentation.Neighborhood;
using SimulationStorm.Avalonia.Extensions;
using SimulationStorm.Primitives;

namespace GenericCellularAutomation.Avalonia.Views;

public partial class CellNeighborhoodView : UserControl
{
    #region Fields
    private CellNeighborhoodViewModel _viewModel = null!;

    private readonly IDictionary<Point, ToggleButton> _positionButtons = new Dictionary<Point, ToggleButton>();

    private bool _skipSelectedPositionsChanged;
    #endregion
    
    public CellNeighborhoodView() => InitializeComponent();

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        if (DataContext is not CellNeighborhoodViewModel viewModel)
            return;

        _viewModel = viewModel;
        
        this.WithDisposables(disposables =>
        {
            _viewModel
                .WhenValueChanged(x => x.Radius)
                .Subscribe(_ => RecreatePositionButtons())
                .DisposeWith(disposables);

            _viewModel
                .WhenValueChanged(x => x.SelectedPositions)
                .Where(_ => !_skipSelectedPositionsChanged)
                .Subscribe(_ => UpdateCheckedPositionButtonsFromViewModel())
                .DisposeWith(disposables);
        });
    }

    private void RecreatePositionButtons()
    {
        _positionButtons.Clear();
        PositionButtonContainer.Children.Clear();
        
        CellNeighborhood.ForEachPositionWithinRadiusIncludingCenter(_viewModel.Radius, position =>
        {
            var positionButton = CreatePositionButton(position);
            _positionButtons[position] = positionButton;
            PositionButtonContainer.Children.Add(positionButton);
        });
    }

    private ToggleButton CreatePositionButton(Point position)
    {
        var positionButton = new ToggleButton
        {
            Tag = position,
            Content = $"({position.X}, {position.Y})",
            IsEnabled = !CellNeighborhood.IsCenterPosition(position)
        };
        
        positionButton.IsCheckedChanged += OnPositionButtonIsCheckedChanged;

        return positionButton;
    }

    private void OnPositionButtonIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        var positionButton = (ToggleButton)sender!;
        var position = (Point)positionButton.Tag!;

        _skipSelectedPositionsChanged = true;
        
        if (positionButton.IsChecked is true)
            _viewModel.SelectPositionCommand.Execute(position);
        else
            _viewModel.UnselectPositionCommand.Execute(position);

        _skipSelectedPositionsChanged = false;
    }

    private void UpdateCheckedPositionButtonsFromViewModel() =>
        CellNeighborhood.ForEachPositionWithinRadius(_viewModel.Radius, position =>
    {
        var positionButton = _positionButtons[position];
        positionButton.IsChecked = _viewModel.SelectedPositions.Contains(position);
    });
}