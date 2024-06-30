using System;

namespace SimulationStorm.Presentation.Navigation;

public class NavigationContentChangedEventArgs(object? previousContent, object newContent) : EventArgs
{
    public object? PreviousContent { get; } = previousContent;

    public object NewContent { get; } = newContent;
}