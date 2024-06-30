using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Data.Converters;
using DynamicData.Binding;
using SimulationStorm.Avalonia.Extensions;
using SimulationStorm.Simulation.History.Presentation.Models;
using SimulationStorm.Simulation.History.Presentation.ViewModels;
using SimulationStorm.ToolPanels.Avalonia;

namespace SimulationStorm.Simulation.History.Avalonia.Views;

public partial class HistoryToolPanelView : ToolPanelControl
{
    public HistoryToolPanelView() => InitializeComponent();

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        
        if (DataContext is not IHistoryViewModel viewModel)
            return;
        
        this.WithDisposables(disposables =>
        {
            RecordsDataGrid
                .GetObservable(DataGrid.SelectedIndexProperty)
                .Where(_ => RecordsDataGrid.IsFocused) // Todo: There may be a case, when this condition will not work as intended...
                .Subscribe(selectedIndex => viewModel.GoToSaveCommand.Execute(selectedIndex))
                .DisposeWith(disposables);
            
            viewModel
                .WhenValueChanged(x => x.CurrentSaveIndex)
                .Subscribe(currentRecordIndex => RecordsDataGrid.SelectedIndex = currentRecordIndex)
                .DisposeWith(disposables);
        });
    }

    private void OnRecordsDataGridLoadingRow(object? _, DataGridRowEventArgs e)
    {
        e.Row.BindClass("is-pointed", new Binding(nameof(HistoryRecordModel.Position))
        {
            Converter = ObjectConverters.Equal,
            ConverterParameter = HistoryRecordPosition.Pointed
        }, null!);
        
        e.Row.BindClass("is-ahead-of-pointer", new Binding(nameof(HistoryRecordModel.Position))
        {
            Converter = ObjectConverters.Equal,
            ConverterParameter = HistoryRecordPosition.AheadOfPointer
        }, null!);
    }
}