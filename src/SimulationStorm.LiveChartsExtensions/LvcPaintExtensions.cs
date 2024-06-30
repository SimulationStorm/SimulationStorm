using LiveChartsCore.SkiaSharpView.Painting;

namespace SimulationStorm.LiveChartsExtensions;

public static class LvcPaintExtensions
{
    public static Paint Clone(this Paint paint) => (Paint)paint.CloneTask();
}