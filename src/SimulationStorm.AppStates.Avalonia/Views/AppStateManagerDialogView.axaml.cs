using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.AppStates.Presentation.Models;
using SimulationStorm.AppStates.Presentation.ViewModels;
using SimulationStorm.Avalonia.Extensions;
using SimulationStorm.DependencyInjection;
using SimulationStorm.Dialogs.Avalonia;
using SimulationStorm.Flyouts.Avalonia;

namespace SimulationStorm.AppStates.Avalonia.Views;

public partial class AppStateManagerDialogView : DialogControl
{
    #region Fields
    private EditAppStateFlyoutViewModel _editAppStateFlyoutViewModel = null!;
    
    private Flyout _editAppStateFlyout = null!;
    #endregion
    
    public AppStateManagerDialogView()
    {
        InitializeComponent();
        
        this.WithDisposables(disposables =>
        {
            var saveAppStateFlyoutViewModel = DiContainer.Default.GetRequiredService<SaveAppStateFlyoutViewModel>();
            FlyoutUtils
                .CreateViewModeledFlyout(saveAppStateFlyoutViewModel, out var saveAppStateFlyout)
                .DisposeWith(disposables);

            saveAppStateFlyout.Placement = PlacementMode.Top;
            SaveAppStateButton.Flyout = saveAppStateFlyout;
            
            _editAppStateFlyoutViewModel = DiContainer.Default.GetRequiredService<EditAppStateFlyoutViewModel>();
            FlyoutUtils
                .CreateViewModeledFlyout(_editAppStateFlyoutViewModel, out _editAppStateFlyout)
                .DisposeWith(disposables);
            
            _editAppStateFlyout.Placement = PlacementMode.Top;

            var deleteAllAppStatesFlyoutViewModel =
                DiContainer.Default.GetRequiredService<DeleteAllAppStatesFlyoutViewModel>()!;
            FlyoutUtils
                .CreateViewModeledFlyout(deleteAllAppStatesFlyoutViewModel, out var deleteAllAppStatesFlyout)
                .DisposeWith(disposables);

            deleteAllAppStatesFlyout.Placement = PlacementMode.Top;
            DeleteAllAppStatesButton.Flyout = deleteAllAppStatesFlyout;
        });
    }

    private void OnRecordModelsDataGridLoadingRow(object? _, DataGridRowEventArgs e)
    {
        e.Row.WithDisposables(disposables =>
        {
            Observable
                .FromEventPattern<EventHandler<TappedEventArgs>, TappedEventArgs>
                (
                    h => e.Row.DoubleTapped += h,
                    h => e.Row.DoubleTapped -= h
                )
                .Subscribe(_ =>
                {
                    if (_editAppStateFlyout.IsOpen)
                        return;
                    
                    var appStateModel = (AppStateModel)e.Row.DataContext!;
                    _editAppStateFlyoutViewModel.AppStateModel = appStateModel;
                    _editAppStateFlyout.ShowAt(e.Row, true);
                })
                .DisposeWith(disposables);
        });
    }
}