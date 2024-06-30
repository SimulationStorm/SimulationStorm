using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ActiproSoftware.UI.Avalonia.Themes;
using Avalonia;
using Avalonia.Media;
using SimulationStorm.Avalonia.Extensions;
using SimulationStorm.Graphics.Avalonia.Extensions;
using SimulationStorm.Presentation.Colors;
using SimulationStorm.Utilities.Disposing;
using Color = SimulationStorm.Graphics.Color;

namespace SimulationStorm.Avalonia.Colors;

public class UiColorProvider : DisposableObject, IUiColorProvider
{
    private static readonly string BackgroundBrushResourceKey =
        ThemeResourceKind.Container1BackgroundBrush.ToResourceKey();
    
    public Color BackgroundColor { get; private set; }
    
    public event EventHandler<UiColorChangedEventArgs>? BackgroundColorChanged;
   
    public UiColorProvider(Application application)
    {
        WithDisposables(disposables =>
        {
            application
                .GetResourceObservableOnUiThread(BackgroundBrushResourceKey)
                .Select(x => x as ISolidColorBrush)
                .Where(x => x is not null)
                .Subscribe(backgroundBrush =>
                {
                    var previousColor = BackgroundColor;
                    BackgroundColor = backgroundBrush!.Color.ToColor();
                    BackgroundColorChanged?.Invoke(this, new UiColorChangedEventArgs(previousColor, BackgroundColor));
                })
                .DisposeWith(disposables);
        });
    }
}