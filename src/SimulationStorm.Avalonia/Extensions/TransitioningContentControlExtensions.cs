using System;
using Avalonia.Controls;

namespace SimulationStorm.Avalonia.Extensions;

public static class TransitioningContentControlExtensions
{
    public static TimeSpan GetTransitionDuration(this TransitioningContentControl transitioningContentControl) =>
        transitioningContentControl.PageTransition?.GetDuration() ?? TimeSpan.Zero;
}