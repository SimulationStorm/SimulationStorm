using System;

namespace SimulationStorm.Presentation.TimeFormatting;

public interface ITimeFormatter
{
    event EventHandler? ReformattingRequested;
    
    string FormatTime(TimeSpan time);
}