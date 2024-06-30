using SimulationStorm.Presentation.TimeFormatting;

namespace SimulationStorm.Avalonia.TimeFormatting;

public static class TimeFormattingConfiguration
{
    public static readonly TimeFormatterOptions TimeFormatterOptions = new()
    {
        DaysStringKey = "Times.Days",
        HoursStringKey = "Times.Hours",
        MinutesStringKey = "Times.Minutes",
        SecondsStringKey = "Times.Seconds",
        MillisecondsStringKey = "Times.Milliseconds",
        MicrosecondsStringKey = "Times.Microseconds",
        NanosecondsStringKey = "Times.Nanoseconds"
    };
}