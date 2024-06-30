using Avalonia;
using Avalonia.Layout;
using Avalonia.Media.Imaging;

namespace SimulationStorm.Avalonia.Extensions;

public static class LayoutableExtensions
{
    public static bool HasAutoWidth(this Layoutable layoutable) => layoutable.Width is double.NaN;

    public static bool HasAutoHeight(this Layoutable layoutable) => layoutable.Height is double.NaN;
    
    public static RenderTargetBitmap RenderToBitmap(this Layoutable layoutable)
    {
        var size = new Size(layoutable.Width, layoutable.Height);
        var sizeInPixels = PixelSize.FromSize(size, 1);
        
        var renderTargetBitmap = new RenderTargetBitmap(sizeInPixels);
        layoutable.Measure(size);
        layoutable.Arrange(new Rect(size));
        renderTargetBitmap.Render(layoutable);
        return renderTargetBitmap;
    }
}