using System;
using System.Linq;
using Avalonia.Animation;

namespace SimulationStorm.Avalonia.Extensions;

public static class PageTransitionExtensions
{
    public static TimeSpan GetDuration(this IPageTransition pageTransition) => pageTransition switch
    {
        CrossFade crossFade => crossFade.Duration,
        PageSlide pageSlide => pageSlide.Duration,
        
        CompositePageTransition compositePageTransition => compositePageTransition.PageTransitions
            .Select(GetDuration)
            .Aggregate((f, s) => f.Add(s)),
        
        _ => throw new NotSupportedException()
    };
}