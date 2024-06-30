using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using DynamicData.Binding;
using SimulationStorm.Avalonia.Extensions;
using SimulationStorm.Graphics.Avalonia.Extensions;
using SimulationStorm.Primitives.Avalonia.Extensions;
using SimulationStorm.Simulation.Presentation;
using SimulationStorm.Simulation.Presentation.Camera;

namespace SimulationStorm.Simulation.Avalonia.Views;

public class WorldView : UserControl
{
    protected bool IsPointerPressed { get; private set; }

    #region Fields
    private WorldViewModel _viewModel = null!;
    
    private readonly Image _worldImage;

    private readonly ISet<Key> _pressedKeys = new HashSet<Key>();
    #endregion

    public WorldView()
    {
        Focusable = true;
        
        _worldImage = new Image();
        Content = _worldImage;
    }

    protected void Initialize(WorldViewModel viewModel)
    {
        _viewModel = viewModel;

        this.WithDisposables(disposables =>
        {
            Observable
                .FromEventPattern<EventHandler<SizeChangedEventArgs>, SizeChangedEventArgs>
                (
                    h => SizeChanged += h,
                    h => SizeChanged -= h
                )
                .Subscribe(e => _viewModel.WorldSize = e.EventArgs.NewSize.ToSize())
                .DisposeWith(disposables);
            
            _viewModel
                .WhenValueChanged(x => x.WorldImage)
                .Subscribe(worldBitmap => _worldImage.Source = worldBitmap?.ToAvalonia())
                .DisposeWith(disposables);
        });
    }

    #region Event handlers
    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        Focus();
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (!e.KeyModifiers.HasFlag(KeyModifiers.Control))
            return;

        _pressedKeys.Add(e.Key);

        if (_pressedKeys.Contains(Key.OemPlus))
            _viewModel.ZoomCameraToViewportCenterCommand.Execute(CameraZoomDirection.ZoomIn);
        
        if (_pressedKeys.Contains(Key.OemMinus))
            _viewModel.ZoomCameraToViewportCenterCommand.Execute(CameraZoomDirection.ZoomOut);

        if (_pressedKeys.Contains(Key.Left))
            _viewModel.TranslateCameraByDirectionCommand.Execute(CameraTranslationDirection.Left);
        
        if (_pressedKeys.Contains(Key.Up))
            _viewModel.TranslateCameraByDirectionCommand.Execute(CameraTranslationDirection.Top);
        
        if (_pressedKeys.Contains(Key.Right))
            _viewModel.TranslateCameraByDirectionCommand.Execute(CameraTranslationDirection.Right);
        
        if (_pressedKeys.Contains(Key.Down))
            _viewModel.TranslateCameraByDirectionCommand.Execute(CameraTranslationDirection.Bottom);
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
        base.OnKeyUp(e);

        _pressedKeys.Remove(e.Key);
    }

    #region Pointer events
    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        IsPointerPressed = true;
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);
        IsPointerPressed = false;
    }

    protected override void OnPointerExited(PointerEventArgs e)
    {
        base.OnPointerExited(e);
        IsPointerPressed = false;
    }

    protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
    {
        base.OnPointerWheelChanged(e);

        if (!e.KeyModifiers.HasFlag(KeyModifiers.Control))
            return;

        var pointerPosition = e.GetPosition(this).ToPointF();

        switch (e.Delta.Y)
        {
            case 1:
                _viewModel.ZoomCameraToPointCommand.Execute((pointerPosition, CameraZoomDirection.ZoomIn));
                break;
            case -1:
                _viewModel.ZoomCameraToPointCommand.Execute((pointerPosition, CameraZoomDirection.ZoomOut));
                break;
        }
    }
    
    protected Point PreviousPointerPosition { get; private set; }
    
    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);

        var pointerPosition = e.GetPosition(this);
        
        if (IsPointerPressed && e.KeyModifiers.HasFlag(KeyModifiers.Control))
        {
            var pointerPositionDelta = (pointerPosition - PreviousPointerPosition).ToVector2();
            _viewModel.TranslateCameraCommand.Execute(pointerPositionDelta);
        }
        
        PreviousPointerPosition = pointerPosition;
    }
    #endregion
    #endregion
}