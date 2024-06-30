using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CommunityToolkit.Mvvm.Input;
using DynamicData.Binding;
using SimulationStorm.Graphics;
using SimulationStorm.Simulation.Bounded.Presentation.Services;
using SimulationStorm.Threading.Presentation;
using SimulationStorm.Utilities.Disposing;

namespace SimulationStorm.Simulation.Bounded.Presentation.ViewModels;

public partial class WorldBackgroundColorViewModel : DisposableObservableObject
{
    public Color BackgroundColor
    {
        get => _worldRenderer.BackgroundColor;
        set => _worldRenderer.BackgroundColor = value;
    }

    #region Commands
    [RelayCommand]
    private void RandomizeBackgroundColor() => BackgroundColor = ColorUtils.GenerateRandomColor();
    
    [RelayCommand(CanExecute = nameof(CanResetBackgroundColor))]
    private void ResetBackgroundColor() => BackgroundColor = _worldRenderer.DefaultBackgroundColor;
    private bool CanResetBackgroundColor() => BackgroundColor != _worldRenderer.DefaultBackgroundColor;
    #endregion

    private readonly IBoundedWorldRenderer _worldRenderer;

    public WorldBackgroundColorViewModel(IUiThreadScheduler uiThreadScheduler, IBoundedWorldRenderer worldRenderer)
    {
        _worldRenderer = worldRenderer;

        WithDisposables(disposables =>
        {
            _worldRenderer
                .WhenValueChanged(x => x.BackgroundColor, false)
                .ObserveOn(uiThreadScheduler)
                .Subscribe(_ =>
                {
                    OnPropertyChanged(nameof(BackgroundColor));
                    ResetBackgroundColorCommand.NotifyCanExecuteChanged();
                })
                .DisposeWith(disposables);
        });
    }
}