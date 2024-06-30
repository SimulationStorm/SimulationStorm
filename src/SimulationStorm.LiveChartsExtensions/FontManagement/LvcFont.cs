using System;

namespace SimulationStorm.LiveChartsExtensions.FontManagement;

public readonly struct LvcFont(string name, LvcFontWeight weight, LvcFontSlant slant) : IEquatable<LvcFont>
{
    // public static readonly LvcFont Default = new(nameof(Default), LvcFontWeight.Regular, LvcFontSlant.Normal);
    
    #region Properties
    public string Name { get; } = name;

    public LvcFontWeight Weight { get; } = weight;
    #endregion

    public LvcFontSlant Slant { get; } = slant;
    
    #region Equality members
    public bool Equals(LvcFont other) =>
        string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase)
        && Weight == other.Weight
        && Slant == other.Slant;

    public override bool Equals(object? obj) => obj is LvcFont other && Equals(other);

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(Name, StringComparer.OrdinalIgnoreCase);
        hashCode.Add((int)Weight);
        hashCode.Add((int)Slant);
        return hashCode.ToHashCode();
    }

    public static bool operator ==(LvcFont left, LvcFont right) => left.Equals(right);

    public static bool operator !=(LvcFont left, LvcFont right) => !left.Equals(right);
    #endregion
}