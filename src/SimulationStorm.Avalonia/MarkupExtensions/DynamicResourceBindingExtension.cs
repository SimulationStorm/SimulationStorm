using System;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;

namespace SimulationStorm.Avalonia.MarkupExtensions;

#pragma warning disable CS0618 // Type or member is obsolete
public class DynamicResourceBindingExtension : BindingExtensionBase
{
    #region Constructors
    public DynamicResourceBindingExtension() { }

    public DynamicResourceBindingExtension(string path) : base(path) { }
    #endregion

    protected override IBinding ProvideValueCore(IServiceProvider serviceProvider)
    {
        var provideValueTarget = serviceProvider.ValidateAndGetProvideValueTarget();
        var targetObject = provideValueTarget.ValidateAndGetTargetObject();
        var resourceHost = ValidateAndGetResourceHost(targetObject);
        var targetProperty = provideValueTarget.ValidateAndGetTargetProperty();
        
        var binding = BuildBinding(serviceProvider);
        return CreateDynamicResourceBinding(binding, targetObject, targetProperty, resourceHost);
    }
    
    #region Private methods
    private static IResourceHost ValidateAndGetResourceHost(AvaloniaObject targetObject)
    {
        if (targetObject is not IResourceHost resourceHost)
            throw new InvalidOperationException(
                $"{nameof(IProvideValueTarget)}.{nameof(IProvideValueTarget.TargetObject)}" +
                $" must implement a {nameof(IResourceHost)}.");

        return resourceHost;
    }

    private static IBinding CreateDynamicResourceBinding(Binding binding, AvaloniaObject targetObject,
        AvaloniaProperty? targetProperty, IResourceHost resourceHost)
    {
        var instancedBinding = binding.Initiate(targetObject, targetProperty)!;
        
        return instancedBinding.Source
            .Select(TransformInitialBindingValue)
            .Where(IsBindingValueNotEmpty)
            .Select(value => value!.ToString())
            .Where(stringifiedValue => !string.IsNullOrWhiteSpace(stringifiedValue))
            .Select(resourceKey => resourceHost.GetResourceObservable(resourceKey!))
            .Switch()
            .ToBinding();
    }
    #endregion
}
#pragma warning restore CS0618 // Type or member is obsolete