using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls.Documents;
using Microsoft.Extensions.DependencyInjection;
using SimulationStorm.Avalonia.Extensions;
using SimulationStorm.DependencyInjection;
using SimulationStorm.Presentation.TimeFormatting;

namespace SimulationStorm.Avalonia.TimeFormatting;

public class TimeRun : Run
{
    public static readonly StyledProperty<TimeSpan> TimeProperty =
        AvaloniaProperty.Register<TimeRun, TimeSpan>(nameof(Time), TimeSpan.Zero);

    public TimeSpan Time
    {
        get => GetValue(TimeProperty);
        set => SetValue(TimeProperty, value);
    }

    private readonly ITimeFormatter _timeFormatter;

    public TimeRun()
    {
        _timeFormatter = DiContainer.Default.GetRequiredService<ITimeFormatter>();

        this.WithDisposables(disposables =>
        {
            Observable
                .FromEventPattern<EventHandler, EventArgs>
                (
                    h => _timeFormatter.ReformattingRequested += h,
                    h => _timeFormatter.ReformattingRequested -= h
                )
                .Subscribe(_ => UpdateText())
                .DisposeWith(disposables);
        });
        
        UpdateText();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == TimeProperty)
            UpdateText();
    }

    private void UpdateText() => Text = _timeFormatter.FormatTime(Time);
}