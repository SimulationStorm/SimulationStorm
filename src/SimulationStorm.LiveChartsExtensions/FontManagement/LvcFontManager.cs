using System;
using System.Collections.Generic;
using SkiaSharp;

namespace SimulationStorm.LiveChartsExtensions.FontManagement;

public class LvcFontManager : IDisposable
{
    public static readonly LvcFontManager Instance = new();

    private readonly IDictionary<LvcFont, SKTypeface> _typefacesByFont = new Dictionary<LvcFont, SKTypeface>();

    // <summary>
    // Sets the typeface that will be associated with <see cref="LvcFont.Default"/>.
    // </summary>
    // public SKTypeface? DefaultTypeface
    // {
    //     get => LiveChartsSkiaSharp.DefaultSKTypeface;
    //     set => LiveChartsSkiaSharp.DefaultSKTypeface = value;
    // }

    private LvcFontManager() { }
    
    public void RegisterTypeface(LvcFont font, SKTypeface typeface)
    {
        if (!_typefacesByFont.TryAdd(font, typeface))
            throw new InvalidOperationException($"The font '{font}' has already been registered.");
    }

    public SKTypeface GetTypeface(LvcFont font)
    {
        // if (font == LvcFont.Default)
        // {
        //     if (DefaultTypeface is null)
        //         throw new InvalidOperationException("The default typeface has not been set.");
        //
        //     return DefaultTypeface;
        // }
        
        if (!_typefacesByFont.TryGetValue(font, out var typeface))
            throw new InvalidOperationException($"The font '{font}' has not been registered.");
        
        return typeface;
    }

    public void Dispose()
    {
        foreach (var (_, typeface) in _typefacesByFont)
            typeface.Dispose();
        
        _typefacesByFont.Clear();
        
        GC.SuppressFinalize(this);
    }
}