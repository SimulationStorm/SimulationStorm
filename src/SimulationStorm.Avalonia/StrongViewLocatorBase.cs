using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace SimulationStorm.Avalonia;

public abstract class StrongViewLocatorBase : IDataTemplate
{
    private readonly IDictionary<Type, Func<Control>> _viewBuilderByViewModelTypes = new Dictionary<Type, Func<Control>>();
    
    protected void Register<TViewModel, TView>() where TView : Control, new() =>
        _viewBuilderByViewModelTypes.Add(typeof(TViewModel), () => new TView());

    public Control Build(object? data)
    {
        if (data is not null && TryGetViewBuilder(data, out var viewBuilder))
            return viewBuilder();

        return new TextBlock { Text = $"A data template for the {data} not found." };
    }

    public bool Match(object? data)
    {
        return data is not null && TryGetViewBuilder(data, out _);
    }

    private bool TryGetViewBuilder(object viewModel, [NotNullWhen(true)] out Func<Control>? viewBuilder)
    {
        var viewModelType = viewModel.GetType();
        
        viewBuilder = null;
        foreach (var (registeredViewModelType, vb) in _viewBuilderByViewModelTypes)
        {
            if (viewModelType.IsAssignableTo(registeredViewModelType))
            {
                viewBuilder = vb;
                break;
            }
        }

        return viewBuilder is not null;
    }
}