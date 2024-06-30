using System;
using System.Reactive;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Data;
using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.Avalonia.MarkupExtensions;
using SimulationStorm.DependencyInjection;
using SimulationStorm.Presentation.TimeFormatting;

namespace SimulationStorm.Avalonia.TimeFormatting;

#pragma warning disable CS0618 // Type or member is obsolete
public class TimeBindingExtension : BindingExtensionBase
{
    #region Constructors
    public TimeBindingExtension() { }

    public TimeBindingExtension(string path) : base(path) { }
    #endregion
    
    protected override IBinding ProvideValueCore(IServiceProvider serviceProvider)
    {
        var provideValueTarget = serviceProvider.ValidateAndGetProvideValueTarget();
        var targetObject = provideValueTarget.ValidateAndGetTargetObject();
        var targetProperty = provideValueTarget.ValidateAndGetTargetProperty();

        var timeFormatter = DiContainer.Default.GetRequiredService<ITimeFormatter>();
        
        var binding = BuildBinding(serviceProvider);
        var instancedBinding = binding.Initiate(targetObject, targetProperty)!;

        // Todo: I think there is a more straightforward and efficient way to do this.
        
        var lastTime = TimeSpan.Zero;
        var timeStream = instancedBinding.Source
            .Select(TransformInitialBindingValue)
            .Where(IsBindingValueNotEmpty)
            .Select(value =>
            {
                if (value is not TimeSpan time)
                    throw new InvalidOperationException($"The binding must produce {nameof(TimeSpan)} values.");

                lastTime = time;
                return time;
            });

        var reformattingRequestedStream = Observable
            .FromEventPattern<EventHandler, EventArgs>
            (
                h => timeFormatter.ReformattingRequested += h,
                h => timeFormatter.ReformattingRequested -= h
            );
        
        return timeStream.Select(_ => Unit.Default)
            .Merge(reformattingRequestedStream.Select(_ => Unit.Default))
            .Select(_ => timeFormatter.FormatTime(lastTime))
            .ToBinding();
    }
}
#pragma warning restore CS0618 // Type or member is obsolete