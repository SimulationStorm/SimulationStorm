using System;
using System.Collections.Generic;
using System.Numerics;
using SimulationStorm.Primitives;
using SimulationStorm.Primitives.Skia;
using SkiaSharp;

namespace SimulationStorm.Graphics.Skia;

public class SkiaCanvas(SKCanvas skCanvas) : ICanvas
{
    public SKCanvas WrappedSkCanvas { get; } = skCanvas;

    #region Methods
    public void SetMatrix(Matrix3x2 matrix) =>
        WrappedSkCanvas.SetMatrix(matrix.ToSkia());
    
    public void ResetMatrix() =>
        WrappedSkCanvas.ResetMatrix();

    public void Scale(float scale) =>
        WrappedSkCanvas.Scale(scale);
    
    public void Clear() =>
        WrappedSkCanvas.Clear();
    
    public void Clear(Color color) =>
        WrappedSkCanvas.Clear(color.ToSkia());

    public void DrawRect(float x, float y, float with, float height, IPaint paint) =>
        WrappedSkCanvas.DrawRect(x, y, with, height, paint.ToSkia());

    public void DrawRect(RectF rect, IPaint paint) =>
        WrappedSkCanvas.DrawRect(rect.ToSkia(), paint.ToSkia());

    public void DrawLine(float x0, float y0, float x1, float y1, IPaint paint) =>
        WrappedSkCanvas.DrawLine(x0, y0, x1, y1, paint.ToSkia());

    public void DrawLine(LineF line, IPaint paint) =>
        WrappedSkCanvas.DrawLine(line.StartX, line.StartY, line.EndX, line.EndY, paint.ToSkia());

    public void DrawBitmap(IBitmap bitmap, PointF position, IPaint? paint = null) =>
        WrappedSkCanvas.DrawBitmap(bitmap.ToSkia(), position.ToSkia(), paint?.ToSkia());

    public void Flush() =>
        WrappedSkCanvas.Flush();
    
    public void Dispose()
    {
        WrappedSkCanvas.Dispose();
        GC.SuppressFinalize(this);
    }

    #region Custom methods
    public void DrawRects(IEnumerable<RectF> rectangles, IPaint paint)
    {
        var skPaint = paint.ToSkia();

        foreach (var rectangle in rectangles)
            WrappedSkCanvas.DrawRect(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom, skPaint);
    }

    public void DrawLines(IEnumerable<LineF> lines, IPaint paint)
    {
        var skPaint = paint.ToSkia();

        foreach (var line in lines)
            WrappedSkCanvas.DrawLine(line.StartX, line.StartY, line.EndX, line.EndY, skPaint);
    }
    #endregion
    #endregion
}