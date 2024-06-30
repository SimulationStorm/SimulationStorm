using System;
using System.Numerics;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimulationStorm.Primitives;
using SimulationStorm.Threading.Presentation;
using SimulationStorm.Utilities.Disposing;

namespace SimulationStorm.Simulation.Presentation.Camera;

public partial class CameraSettingsViewModel : DisposableObservableObject
{
    #region Properties
    public float Zoom
    {
        get => _worldCamera.Zoom;
        set
        {
            if (Math.Abs(value - _worldCamera.Zoom) > 0.001f)            
                _worldCamera.ZoomToViewportCenter(value);
        }
    }
    
    public float HorizontalTranslation
    {
        get => _worldCamera.Translation.X;
        set
        {
            if (Math.Abs(value - _worldCamera.Translation.X) > 0.001f)
                _worldCamera.SetTranslation(_worldCamera.Translation with { X = value });
        }
    }

    public float VerticalTranslation
    {
        get => _worldCamera.Translation.Y;
        set
        {
            if (Math.Abs(value - _worldCamera.Translation.Y) > 0.001f)
                _worldCamera.SetTranslation(_worldCamera.Translation with { Y = value });
        }
    }

    public float ZoomChange
    {
        get => _worldCamera.ZoomChange;
        set
        {
            if (Math.Abs(value - _worldCamera.ZoomChange) < 0.001f)
                return;
            
            _worldCamera.ZoomChange = value;
            OnPropertyChanged();
            ResetZoomChangeCommand.NotifyCanExecuteChanged();
        }
    }

    public float TranslationChange
    {
        get => _worldCamera.TranslationChange;
        set
        {
            if (Math.Abs(value - _worldCamera.TranslationChange) < 0.001f)
                return;
            
            _worldCamera.TranslationChange = value;
            OnPropertyChanged();
            ResetTranslationChangeCommand.NotifyCanExecuteChanged();
        }
    }
    
    [ObservableProperty] private CameraMovingMethod _movingMethod;

    public Range<float> ZoomRange => _options.ZoomRange;

    public Range<float> ZoomChangeRange => _options.ZoomChangeRange;

    public Range<PointF> TranslationRange => _options.TranslationRange;
    
    public Range<float> TranslationChangeRange => _options.TranslationChangeRange;
    #endregion

    #region Commands
    [RelayCommand(CanExecute = nameof(CanChangeMovingMethod))]
    private void ChangeMovingMethod(CameraMovingMethod movingMethod)
    {
        MovingMethod = movingMethod;
        ChangeMovingMethodCommand.NotifyCanExecuteChanged();
    }
    private bool CanChangeMovingMethod(CameraMovingMethod movingMethod) => movingMethod != MovingMethod;

    [RelayCommand(CanExecute = nameof(CanResetZoom))]
    private void ResetZoom() => _worldCamera.ResetZoom();
    private bool CanResetZoom() => _worldCamera.CanResetZoom();

    [RelayCommand]
    private void Translate(Vector2 offset) => _worldCamera.Translate(offset);
    
    [RelayCommand]
    private void TranslateInDirection(CameraTranslationDirection translationDirection) =>
        _worldCamera.Translate(translationDirection);

    [RelayCommand(CanExecute = nameof(CanResetTranslation))]
    private void ResetTranslation() => _worldCamera.ResetTranslation();
    private bool CanResetTranslation() => _worldCamera.CanResetTranslation();

    [RelayCommand(CanExecute = nameof(CanResetZoomChange))]
    private void ResetZoomChange() => ZoomChange = _options.DefaultZoomChange;
    private bool CanResetZoomChange() => Math.Abs(ZoomChange - _options.DefaultZoomChange) > 0.001f;
    
    [RelayCommand(CanExecute = nameof(CanResetTranslationChange))]
    private void ResetTranslationChange() => TranslationChange = _options.DefaultTranslationChange;
    private bool CanResetTranslationChange() => Math.Abs(TranslationChange - _options.DefaultTranslationChange) > 0.001f;
    #endregion

    #region Fields
    private readonly IWorldCamera _worldCamera;

    private readonly WorldCameraOptions _options;
    #endregion

    public CameraSettingsViewModel(IUiThreadScheduler uiThreadScheduler, IWorldCamera worldCamera, WorldCameraOptions options)
    {
        _worldCamera = worldCamera;
        _options = options;
        
        WithDisposables(disposables =>
        {
            Observable
                .FromEventPattern<EventHandler<CameraMatrixChangedEventArgs>, CameraMatrixChangedEventArgs>
                (
                    h => _worldCamera.MatrixChanged += h,
                    h => _worldCamera.MatrixChanged -= h
                )
                .ObserveOn(uiThreadScheduler)
                .Subscribe(_ =>
                {
                    OnPropertyChanged(nameof(Zoom));
                    OnPropertyChanged(nameof(HorizontalTranslation));
                    OnPropertyChanged(nameof(VerticalTranslation));
                    
                    ResetZoomCommand.NotifyCanExecuteChanged();
                    ResetTranslationCommand.NotifyCanExecuteChanged();
                })
                .DisposeWith(disposables);
        });
    }
}