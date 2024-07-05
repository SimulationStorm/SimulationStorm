using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using SimulationStorm.Localization.Presentation;
using SimulationStorm.Utilities.Disposing;

namespace SimulationStorm.Presentation.TimeFormatting;

public class TimeFormatter : DisposableObject, ITimeFormatter
{
    public event EventHandler? ReformattingRequested;

    #region Fields
    private readonly ILocalizationManager _localizationManager;

    private readonly TimeFormatterOptions _options;
    
    private string _daysString = null!,
                   _hoursString = null!,
                   _minutesString = null!,
                   _secondsString = null!,
                   _millisecondsString = null!,
                   _microsecondsString = null!,
                   _nanosecondsString = null!;
    #endregion

    public TimeFormatter(ILocalizationManager localizationManager, TimeFormatterOptions options)
    {
        _localizationManager = localizationManager;
        _options = options;
        
        Observable
            .FromEventPattern<EventHandler<CultureChangedEventArgs>, CultureChangedEventArgs>
            (
                h => localizationManager.CultureChanged += h,
                h => localizationManager.CultureChanged -= h
            )
            .Subscribe(_ =>
            {
                UpdateStrings();
                NotifyReformattingRequested();
            })
            .DisposeWith(Disposables);
        
        UpdateStrings();
    }

    public string FormatTime(TimeSpan time) =>
        time.Days > 0 ?
            time.Hours > 0 ? $"{GetDaysPart(time)}, {GetHoursPart(time)}" : GetDaysPart(time) :
        time.Hours > 0 ?
            time.Minutes > 0 ? $"{GetHoursPart(time)}, {GetMinsPart(time)}" : GetHoursPart(time) :
        time.Minutes > 0 ?
            time.Seconds > 0 ? $"{GetMinsPart(time)}, {GetSecPart(time)}" : GetMinsPart(time) :
        time.Seconds > 0 ?
            time.Milliseconds > 0 ? $"{GetSecPart(time)}, {GetMsPart(time)}" : GetSecPart(time) :
        time.Milliseconds > 0 ?
            time.Microseconds > 0 ? $"{GetMsPart(time)}, {GetMcsPart(time)}" : GetMsPart(time) :
        time.Microseconds > 0 ?
            time.Nanoseconds > 0 ? $"{GetMcsPart(time)}, {GetNsPart(time)}" : GetMcsPart(time) :
        time.Nanoseconds > 0 ?
            GetNsPart(time) : "0";

    #region Private methods
    private void UpdateStrings()
    {
        _daysString = GetLocalizedString(_options.DaysStringKey);
        _hoursString = GetLocalizedString(_options.HoursStringKey);
        _minutesString = GetLocalizedString(_options.MinutesStringKey);
        _secondsString = GetLocalizedString(_options.SecondsStringKey);
        _millisecondsString = GetLocalizedString(_options.MillisecondsStringKey);
        _microsecondsString = GetLocalizedString(_options.MicrosecondsStringKey);
        _nanosecondsString = GetLocalizedString(_options.NanosecondsStringKey);
    }
    
    private void NotifyReformattingRequested() => ReformattingRequested?.Invoke(this, EventArgs.Empty);

    private string GetLocalizedString(string key) => _localizationManager.GetLocalizedString(key);

    private string GetDaysPart(TimeSpan timeSpan) => $"{timeSpan.Days} {_daysString}";
    private string GetHoursPart(TimeSpan timeSpan) => $"{timeSpan.Hours} {_hoursString}";
    private string GetMinsPart(TimeSpan timeSpan) => $"{timeSpan.Minutes} {_minutesString}";
    private string GetSecPart(TimeSpan timeSpan) => $"{timeSpan.Seconds} {_secondsString}";
    private string GetMsPart(TimeSpan timeSpan) => $"{timeSpan.Milliseconds} {_millisecondsString}";
    private string GetMcsPart(TimeSpan timeSpan) => $"{timeSpan.Microseconds} {_microsecondsString}";
    private string GetNsPart(TimeSpan timeSpan) => $"{timeSpan.Nanoseconds} {_nanosecondsString}";
    #endregion
}