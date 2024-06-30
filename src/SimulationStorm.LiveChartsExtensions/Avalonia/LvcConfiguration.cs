using System;
using System.Collections.Generic;
using LiveChartsCore;
using LiveChartsCore.Drawing;
using LiveChartsCore.Themes;
using SimulationStorm.LiveChartsExtensions.FontManagement;
using SimulationStorm.LiveChartsExtensions.ThemeManagement;
using SimulationStorm.LiveChartsExtensions.ThemeManagement.Options;

namespace SimulationStorm.LiveChartsExtensions.Avalonia;

public static class LvcConfiguration
{
    public static readonly LvcOptions LvcOptions = new()
    {
        FontsOptions = new LvcFontsOptions
        {
            FontResourcesAssemblyName = "SimulationStorm.LiveChartsExtensions",
            FontResourcesDirectoryPath = "Avalonia/Resources/Fonts",
            FontResourceExtension = "ttf",
            FontsByFontResourceNames = new Dictionary<string, LvcFont>
            {
                ["Nunito-Regular"] = new("Nunito", LvcFontWeight.Regular, LvcFontSlant.Normal),
                ["Nunito-SemiBold"] = new("Nunito", LvcFontWeight.SemiBold, LvcFontSlant.Normal),
            },
            DefaultFont = new LvcFont("Nunito", LvcFontWeight.Regular, LvcFontSlant.Normal)
        },
        ThemesOptions = new LvcThemesOptions
        {
            ThemeOptionsByTheme = new Dictionary<LvcTheme, LvcThemeOptions>
            {
                [LvcTheme.Dark] = new()
                {
                    AnimationsSpeed = TimeSpan.FromSeconds(0.125),
                    EasingFunction = EasingFunctions.ExponentialOut,
                    ColorPalette = ColorPalletes.MaterialDesign200,
                    
                    LegendOptions = new LvcLegendOptions
                    {
                        Font = new LvcFont("Nunito", LvcFontWeight.SemiBold, LvcFontSlant.Normal),
                        FontSize = 13,
                        ForegroundColor = new LvcColor(239, 239, 241), // DefaultForegroundBrush
                    },
                    
                    TooltipOptions = new LvcTooltipOptions
                    {
                        Font = new LvcFont("Nunito", LvcFontWeight.SemiBold, LvcFontSlant.Normal),
                        FontSize = 13,
                        ForegroundColor = new LvcColor(239, 239, 241), // DefaultForegroundBrush
                        BackgroundColor = new LvcColor(45, 48, 55) // Container3BackgroundBrush
                    },
                    
                    AxisOptions = new LvcAxisOptions
                    {
                        NameFont = new LvcFont("Nunito", LvcFontWeight.SemiBold, LvcFontSlant.Normal),
                        NameFontSize = 14,
                        NameColor = new LvcColor(159, 163, 173), // HeadingForegroundBrushMedium
                        
                        LabelsFont = new LvcFont("Nunito", LvcFontWeight.Regular, LvcFontSlant.Normal),
                        LabelsFontSize = 13,
                        LabelsColor = new LvcColor(239, 239, 241), // DefaultForegroundBrush
                        
                        AreSeparatorLinesVisible = true,
                        SeparatorsColor = new LvcColor(204, 206, 211) // ControlForegroundBrushSolidDisabled
                    },
                    
                    LineSeriesOptions = new LvcLineSeriesOptions
                    {
                        StrokeSize = 1.5f,
                        GeometryOptions = new LvcGeometryOptions
                        {
                            Size = 1.5f,
                            StrokeSize = 0,
                            FillColor = new LvcColor(255, 255, 255) // DefaultBackgroundBrush
                        }
                    },
                    
                    BarSeriesOptions = new LvcBarSeriesOptions
                    {
                        CornerRadiusOnXAxis = 3,
                        CornerRadiusOnYAxis = 3
                    }
                },
                [LvcTheme.Light] = new()
                {
                    AnimationsSpeed = TimeSpan.FromSeconds(0.125),
                    EasingFunction = EasingFunctions.ExponentialOut,
                    ColorPalette = ColorPalletes.MaterialDesign500,
                    
                    LegendOptions = new LvcLegendOptions
                    {
                        Font = new LvcFont("Nunito", LvcFontWeight.SemiBold, LvcFontSlant.Normal),
                        FontSize = 13,
                        ForegroundColor = new LvcColor(45, 48, 55), // DefaultForegroundBrush
                    },
                    
                    TooltipOptions = new LvcTooltipOptions
                    {
                        Font = new LvcFont("Nunito", LvcFontWeight.SemiBold, LvcFontSlant.Normal),
                        FontSize = 13,
                        ForegroundColor = new LvcColor(45, 48, 55), // DefaultForegroundBrush
                        BackgroundColor = new LvcColor(239, 239, 241) // Container3BackgroundBrush
                    },
                    
                    AxisOptions = new LvcAxisOptions
                    {
                        NameFont = new LvcFont("Nunito", LvcFontWeight.SemiBold, LvcFontSlant.Normal),
                        NameFontSize = 14,
                        NameColor = new LvcColor(108, 114, 129), // HeadingForegroundBrushMedium
                        
                        LabelsFont = new LvcFont("Nunito", LvcFontWeight.Regular, LvcFontSlant.Normal),
                        LabelsFontSize = 13,
                        LabelsColor = new LvcColor(45, 48, 55), // DefaultForegroundBrush
                        
                        AreSeparatorLinesVisible = true,
                        SeparatorsColor = new LvcColor(204, 206, 211) // ControlForegroundBrushSolidDisabled
                    },
                    
                    LineSeriesOptions = new LvcLineSeriesOptions
                    {
                        StrokeSize = 1.5f,
                        GeometryOptions = new LvcGeometryOptions
                        {
                            Size = 3f,
                            StrokeSize = 1.5f,
                            FillColor = new LvcColor(255, 255, 255) // DefaultBackgroundBrush
                        }
                    },
                    
                    BarSeriesOptions = new LvcBarSeriesOptions
                    {
                        CornerRadiusOnXAxis = 3,
                        CornerRadiusOnYAxis = 3
                    }
                }
            },
            DefaultTheme = LvcTheme.Light
        }
    };
}