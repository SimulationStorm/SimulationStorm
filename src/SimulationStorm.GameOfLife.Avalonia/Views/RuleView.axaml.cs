using System;
using System.Globalization;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Input;
using DotNext.Collections.Generic;
using DynamicData.Binding;
using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.Avalonia.Controls;
using SimulationStorm.Avalonia.Extensions;
using SimulationStorm.DependencyInjection;
using SimulationStorm.GameOfLife.DataTypes;
using SimulationStorm.GameOfLife.Presentation.Rules;

namespace SimulationStorm.GameOfLife.Avalonia.Views;

public partial class RuleView : Section
{
    public RuleView()
    {
        InitializeComponent();

        var viewModel = DiContainer.Default.GetRequiredService<RuleViewModel>();
        DataContext = viewModel;
        
        var bornNeighborCountConverter = new RuleNeighborCountToBoolConverter(
            RuleNeighborCountType.Born, () => viewModel.EditingRule);
        var surviveNeighborCountConverter = new RuleNeighborCountToBoolConverter(
            RuleNeighborCountType.Survive, () => viewModel.EditingRule);

        Enumerable
            .Range(GameOfLifeRule.MinNeighborCount, GameOfLifeRule.MaxNeighborCount + 1)
            .ForEach(neighborCount =>
            {
                BornNeighborCountButtonsPanel.Children.Add(
                    CreateNeighborCountToggleButton(neighborCount, bornNeighborCountConverter));
                SurviveNeighborCountButtonsPanel.Children.Add(
                    CreateNeighborCountToggleButton(neighborCount, surviveNeighborCountConverter));
            });
        
        this.WithDisposables(disposables =>
        {
            var skipRuleChangedNotification = false;
            
            PredefinedRulesTreeView
                .GetObservable(SelectingItemsControl.SelectedItemProperty)
                .Where(selectedItem => selectedItem is not null)
                .Subscribe(selectedItem =>
                {
                    if (selectedItem is not NamedRule namedRule || namedRule.Rule.Equals(viewModel.EditingRule))
                        return;

                    skipRuleChangedNotification = true;
                    viewModel.EditingRule = namedRule.Rule;
                })
                .DisposeWith(disposables);
            
            viewModel
                .WhenValueChanged(x => x.EditingRule, false)
                .Subscribe(_ =>
                {
                    if (skipRuleChangedNotification)
                        skipRuleChangedNotification = false;
                    else
                        PredefinedRulesTreeView.SelectedItem = null;
                })
                .DisposeWith(disposables);
        });
    }
    
    private static ToggleButton CreateNeighborCountToggleButton(int neighborCount, IValueConverter neighborCountConverter)
    {
        var neighborCountToggleButton = new ToggleButton
        {
            Content = neighborCount,
            [!ToggleButton.IsCheckedProperty] = new Binding(nameof(RuleViewModel.EditingRule))
            {
                Converter = neighborCountConverter,
                ConverterParameter = neighborCount
            }
        };

        neighborCountToggleButton.BindClass("accent", new Binding(nameof(ToggleButton.IsChecked))
        {
            RelativeSource = new RelativeSource(RelativeSourceMode.Self)
        }, null!);

        return neighborCountToggleButton;
    }

    public enum RuleNeighborCountType
    {
        Born,
        Survive
    }
    
    public class RuleNeighborCountToBoolConverter
    (
        RuleNeighborCountType neighborCountType,
        Func<GameOfLifeRule> ruleGetter
    )
        : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not GameOfLifeRule rule)
                return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
        
            if (parameter is not int neighborCount)
                return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);

#pragma warning disable CS8524
            return neighborCountType switch
#pragma warning restore CS8524
            {
                RuleNeighborCountType.Born => rule.IsBornWhen(neighborCount),
                RuleNeighborCountType.Survive => rule.IsSurviveWhen(neighborCount)
            };
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not bool isSelected)
                return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
        
            if (parameter is not int neighborCount)
                return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);

            var rule = ruleGetter();
#pragma warning disable CS8846
            return isSelected switch
#pragma warning restore CS8846
            {
                true when neighborCountType is RuleNeighborCountType.Born => rule.WithNeighborCountToBorn(neighborCount),
                false when neighborCountType is RuleNeighborCountType.Born => rule.WithoutNeighborCountToBorn(neighborCount),
                true when neighborCountType is RuleNeighborCountType.Survive => rule.WithNeighborCountToSurvive(neighborCount),
                false when neighborCountType is RuleNeighborCountType.Survive => rule.WithoutNeighborCountToSurvive(neighborCount)
            };
        }
    }
}