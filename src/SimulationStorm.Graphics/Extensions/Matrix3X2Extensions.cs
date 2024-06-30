using System.Numerics;

namespace SimulationStorm.Graphics.Extensions;

/// <summary>
/// Provides methods to simplify work with the <see cref="Matrix3x2"/>.
/// </summary>
public static class Matrix3X2Extensions
{
    /// <summary>
    /// Gets the matrix scale.
    /// </summary>
    /// <param name="matrix">The matrix.</param>
    /// <returns>The matrix scale.</returns>
    public static Vector2 GetScale(this Matrix3x2 matrix) => new(matrix.M11, matrix.M22);

    /// <summary>
    /// Sets the matrix scale to the <see cref="scale"/>.
    /// </summary>
    /// <param name="matrix">The matrix.</param>
    /// <param name="scale">The new scale of the matrix.</param>
    public static void SetScale(this ref Matrix3x2 matrix, Vector2 scale)
    {
        matrix.M11 = scale.X;
        matrix.M22 = scale.Y;
    }

    /// <summary>
    /// Scales the matrix to the <see cref="amount"/>.
    /// </summary>
    /// <param name="matrix">The matrix.</param>
    /// <param name="amount">The amount to scale the matrix to.</param>
    public static void Scale(this ref Matrix3x2 matrix, Vector2 amount) => matrix *= Matrix3x2.CreateScale(amount);

    /// <summary>
    /// Gets the matrix translation.
    /// </summary>
    /// <param name="matrix">The matrix.</param>
    /// <returns>The matrix translation.</returns>
    public static Vector2 GetTranslation(this Matrix3x2 matrix) => matrix.Translation;

    /// <summary>
    /// Sets the matrix translation to the <see cref="translation"/>.
    /// </summary>
    /// <param name="matrix">The matrix.</param>
    /// <param name="translation">The new translation of the matrix.</param>
    public static void SetTranslation(this ref Matrix3x2 matrix, Vector2 translation) => matrix.Translation = translation;

    /// <summary>
    /// Translates the matrix by the <see cref="offset"/>.
    /// </summary>
    /// <param name="matrix">The matrix.</param>
    /// <param name="offset">The offset to translate the matrix by.</param>
    public static void Translate(this ref Matrix3x2 matrix, Vector2 offset) => matrix.Translation += offset;
}