using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls.Primitives;
using DynamicData.Binding;
using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.Avalonia.Controls;
using SimulationStorm.Avalonia.Extensions;
using SimulationStorm.DependencyInjection;
using SimulationStorm.GameOfLife.Presentation.Patterns;

namespace SimulationStorm.GameOfLife.Avalonia.Views;

public partial class PatternsView : Section
{
    public PatternsView()
    {
        InitializeComponent();

        var viewModel = DiContainer.Default.GetRequiredService<PatternsViewModel>();
        DataContext = viewModel;
        
        this.WithDisposables(disposables =>
        {
            PatternsTreeView
                .GetObservable(SelectingItemsControl.SelectedItemProperty)
                .Where(selectedItem => selectedItem is not null)
                .Subscribe(selectedItem =>
                {
                    if (selectedItem is not NamedPattern namedPattern)
                        return;
                    
                    viewModel.CurrentPattern = namedPattern;
                })
                .DisposeWith(disposables);
            
            viewModel
                .WhenValueChanged(x => x.CurrentPattern, false)
                .Subscribe(pattern => PatternsTreeView.SelectedItem = pattern)
                .DisposeWith(disposables);
        });
    }
}