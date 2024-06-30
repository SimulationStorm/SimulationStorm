using System;
using System.Collections.Generic;
using System.Numerics;
using SimulationStorm.Primitives;

namespace SimulationStorm.Graphics;

/// <summary>
/// Encapsulates all the state about drawing into a bitmap.
/// </summary>
public interface ICanvas : IDisposable
{
    /// <summary>
    /// Replaces the current matrix with a copy of the specified matrix.
    /// </summary>
    /// <param name="matrix">The matrix that will be copied into the current matrix.</param>
    void SetMatrix(Matrix3x2 matrix);

    /// <summary>
    /// Sets the current matrix to identity.
    /// </summary>
    void ResetMatrix();

    /// <summary>
    /// Pre-concatenates the current matrix with the specified scale.
    /// </summary>
    /// <param name="scale">The amount to scale.</param>
    public void Scale(float scale);

    /// <summary>
    /// Replaces all the pixels in the canvas' current clip with the <see cref="Color.Empty" /> color.
    /// </summary>
    void Clear();
    
    /// <summary>
    /// Replaces all the pixels in the canvas' current clip with the specified color.
    /// </summary>
    /// <param name="color">The color to use to replace the pixels in the current clipping region.</param>
    void Clear(Color color);

    /// <summary>
    /// Draws a rectangle on the canvas.
    /// </summary>
    /// <param name="x">The x-coordinate.</param>
    /// <param name="y">The y-coordinate.</param>
    /// <param name="width">The rectangle width.</param>
    /// <param name="height">The rectangle height.</param>
    /// <param name="paint">The paint to use when drawing the rectangle.</param>
    void DrawRect(float x, float y, float width, float height, IPaint paint);

    /// <summary>
    /// Draws a rectangle on the canvas.
    /// </summary>
    /// <param name="rect">The rectangle to draw.</param>
    /// <param name="paint">The paint to use when drawing the rectangle.</param>
    void DrawRect(RectF rect, IPaint paint);

    /// <summary>
    /// Draws a line on the canvas.
    /// </summary>
    /// <param name="x0">The first point x-coordinate.</param>
    /// <param name="y0">The first point y-coordinate.</param>
    /// <param name="x1">The second point x-coordinate.</param>
    /// <param name="y1">The second point y-coordinate.</param>
    /// <param name="paint">The paint to use when drawing the line.</param>
    void DrawLine(float x0, float y0, float x1, float y1, IPaint paint);
    
    /// <summary>
    /// Draws a line on the canvas.
    /// </summary>
    /// <param name="line">The line.</param>
    /// <param name="paint">The paint to use when drawing the line.</param>
    void DrawLine(LineF line, IPaint paint);

    /// <summary>
    /// Draws a bitmap on the canvas.
    /// </summary>
    /// <param name="bitmap">The bitmap to draw.</param>
    /// <param name="position">The destination coordinates for the bitmap.</param>
    /// <param name="paint">The paint to use when drawing the bitmap.</param>
    void DrawBitmap(IBitmap bitmap, PointF position, IPaint? paint = null);

    /// <summary>
    /// Triggers the immediate execution of all pending draw operations.
    /// </summary>
    void Flush();

    #region Custom methods
    /// <summary>
    /// Draws a collection of rectangles on the canvas.
    /// </summary>
    /// <param name="rectangles">The rectangles to draw.</param>
    /// <param name="paint">The paint to use when drawing the rectangles.</param>
    void DrawRects(IEnumerable<RectF> rectangles, IPaint paint);
    
    /// <summary>
    /// Draws a collection of lines on the canvas.
    /// </summary>
    /// <param name="lines">The lines to draw.</param>
    /// <param name="paint">The paint to use when drawing the lines.</param>
    void DrawLines(IEnumerable<LineF> lines, IPaint paint);
    #endregion
}