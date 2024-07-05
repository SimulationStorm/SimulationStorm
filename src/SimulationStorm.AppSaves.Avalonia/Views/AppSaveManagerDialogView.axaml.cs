using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.AppSaves.Presentation.Models;
using SimulationStorm.AppSaves.Presentation.ViewModels;
using SimulationStorm.Avalonia.Extensions;
using SimulationStorm.DependencyInjection;
using SimulationStorm.Dialogs.Avalonia;
using SimulationStorm.Flyouts.Avalonia;

namespace SimulationStorm.AppSaves.Avalonia.Views;

public partial class AppSaveManagerDialogView : DialogControl
{
    #region Fields
    private EditAppSaveFlyoutViewModel _editAppSaveFlyoutViewModel = null!;
    
    private Flyout _editAppSaveFlyout = null!;
    #endregion
    
    public AppSaveManagerDialogView()
    {
        InitializeComponent();
        
        this.WithDisposables(disposables =>
        {
            var saveAppFlyoutViewModel = DiContainer.Default.GetRequiredService<SaveAppFlyoutViewModel>();
            FlyoutUtils
                .CreateViewModeledFlyout(saveAppFlyoutViewModel, out var saveAppFlyout)
                .DisposeWith(disposables);

            saveAppFlyout.Placement = PlacementMode.Top;
            SaveAppButton.Flyout = saveAppFlyout;
            
            _editAppSaveFlyoutViewModel = DiContainer.Default.GetRequiredService<EditAppSaveFlyoutViewModel>();
            FlyoutUtils
                .CreateViewModeledFlyout(_editAppSaveFlyoutViewModel, out _editAppSaveFlyout)
                .DisposeWith(disposables);
            
            _editAppSaveFlyout.Placement = PlacementMode.Top;

            var deleteAllAppSavesFlyoutViewModel =
                DiContainer.Default.GetRequiredService<DeleteAllAppSavesFlyoutViewModel>();
            FlyoutUtils
                .CreateViewModeledFlyout(deleteAllAppSavesFlyoutViewModel, out var deleteAllAppSavesFlyout)
                .DisposeWith(disposables);

            deleteAllAppSavesFlyout.Placement = PlacementMode.Top;
            DeleteAllAppSavesButton.Flyout = deleteAllAppSavesFlyout;
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
                    if (_editAppSaveFlyout.IsOpen)
                        return;
                    
                    var appSaveModel = (AppSaveModel)e.Row.DataContext!;
                    _editAppSaveFlyoutViewModel.AppSaveModel = appSaveModel;
                    _editAppSaveFlyout.ShowAt(e.Row, true);
                })
                .DisposeWith(disposables);
        });
    }
}