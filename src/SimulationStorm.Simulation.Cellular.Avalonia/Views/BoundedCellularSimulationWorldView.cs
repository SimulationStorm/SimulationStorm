using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Input;
using SimulationStorm.Primitives;
using SimulationStorm.Primitives.Avalonia.Extensions;
using SimulationStorm.Simulation.Avalonia.Views;
using SimulationStorm.Simulation.Cellular.Presentation.Models;
using SimulationStorm.Simulation.Cellular.Presentation.ViewModels;

namespace SimulationStorm.Simulation.Cellular.Avalonia.Views;

public class BoundedCellularSimulationWorldView : WorldView
{
    protected readonly List<Point> PressedCellsHistory = [];
    
    private BoundedCellularSimulationWorldViewModel _viewModel = null!;

    protected void Initialize(BoundedCellularSimulationWorldViewModel viewModel)
    {
        _viewModel = viewModel;
        base.Initialize(viewModel);
    }

    #region Cell event handlers
    protected virtual void OnCellHovered(Point cell, KeyModifiers keyModifiers) =>
        _viewModel.HighlightCellCommand.Execute((cell, CellularWorldPointedCellState.Hovered));

    protected virtual void OnCellPressed(Point cell, KeyModifiers keyModifiers)
    {
        _viewModel.HighlightCellCommand.Execute((cell, CellularWorldPointedCellState.Pressed));
        
        if (!keyModifiers.HasFlag(KeyModifiers.Control))
            PressedCellsHistory.Add(cell);
    }
    
    protected virtual void OnCellReleased() { }
    
    protected virtual void OnCellCaptureLost() =>
        _viewModel.UnhighlightCellCommand.Execute(null);
    #endregion

    #region Event handlers
    protected override void OnPointerExited(PointerEventArgs e)
    {
        base.OnPointerExited(e);
        OnCellCaptureLost();
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        var pointerPosition = e.GetPosition(this).ToPoint();
        HandleCellUnderPointer(pointerPosition, e.KeyModifiers);
    }
    
    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);
        
        var pointerPosition = e.GetPosition(this).ToPoint();
        HandleCellUnderPointer(pointerPosition, e.KeyModifiers);
        OnCellReleased();
    }
    
    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);
        
        var pointerPosition = e.GetPosition(this).ToPoint();
        HandleCellUnderPointer(pointerPosition, e.KeyModifiers);
    }
    #endregion

    private void HandleCellUnderPointer(Point pointerPosition, KeyModifiers keyModifiers)
    {
        if (_viewModel.CameraZoom >= 1 && _viewModel.TryGetCellUnderPoint(pointerPosition, out var cell))
        {
            if (IsPointerPressed)
                OnCellPressed(cell.Value, keyModifiers);
            else
                OnCellHovered(cell.Value, keyModifiers);
        }
        else
            OnCellCaptureLost();
    }
    
    #region Protected methods
    protected IEnumerable<Point> ConnectPreviousPressedCellsToLinesAndGetPoints() =>
        ConnectPointsToLines(PressedCellsHistory)
            .Select(GetLinePoints)
            .SelectMany(x => x);
    
    protected static IEnumerable<Point> GetLinePoints((Point From, Point To) line)
    {
        int xFrom = line.From.X,
            yFrom = line.From.Y,
            xTo = line.To.X,
            yTo = line.To.Y,
            deltaX = Math.Abs(xTo - xFrom),
            deltaY = Math.Abs(yTo - yFrom),
            signX = xFrom < xTo ? 1 : -1,
            signY = yFrom < yTo ? 1 : -1,
            error = deltaX - deltaY,
            currentX = xFrom,
            currentY = yFrom;

        while (currentX != xTo || currentY != yTo)
        {
            yield return new Point(currentX, currentY);

            var doubledError = error * 2;

            if (doubledError > -deltaY)
            {
                error -= deltaY;
                currentX += signX;
            }

            if (doubledError < deltaX)
            {
                error += deltaX;
                currentY += signY;
            }
        }

        yield return new Point(xTo, yTo); // The last cell at (xTo, yTo)
    }

    protected static IEnumerable<(Point From, Point To)> ConnectPointsToLines(IReadOnlyList<Point> points)
    {
        if (points.Count is 0)
            yield break;
        
        if (points.Count is 1)
            yield return (points[0], points[0]);

        for (var i = 1; i < points.Count; i++)
            yield return (points[i - 1], points[i]);
    }
    #endregion
}