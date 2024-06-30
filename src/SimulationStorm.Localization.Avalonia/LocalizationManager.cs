using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using SimulationStorm.Avalonia.Extensions;
using SimulationStorm.Localization.Presentation;
using SimulationStorm.Utilities.Disposing;

namespace SimulationStorm.Localization.Avalonia;

/// <inheritdoc cref="ILocalizationManager"/>
public class LocalizationManager : DisposableObject, ILocalizationManager
{
    #region Properties
    /// <inheritdoc/>
    public IEnumerable<CultureInfo> SupportedCultures => _localeResourceProvider.AvailableCultures;

    /// <inheritdoc/>
    public CultureInfo CurrentCulture { get; private set; } = null!;
    #endregion

    public event EventHandler<CultureChangedEventArgs>? CultureChanged;

    #region Fields
    private readonly IResourceDictionary _localeResourceDictionary;

    private readonly ILocaleResourceProvider _localeResourceProvider;

    private readonly string _stringResourceKeyPrefix;

    private bool _skipCultureManagerCultureChangedNotification;
    #endregion

    public LocalizationManager
    (
        IResourceDictionary localeResourceDictionary,
        ILocaleResourceProvider localeResourceProvider,
        LocalizationOptions options)
    {
        _localeResourceDictionary = localeResourceDictionary;
        _localeResourceProvider = localeResourceProvider;
        _stringResourceKeyPrefix = options.StringResourceKeyPrefix;

        var initialCulture = GetInitialCulture(options);
        SetCurrentCultureAndChangeCultureManagerCulture(initialCulture);
        AddLocaleResourcesToResourceDictionary(initialCulture);
        
        Observable
            .FromEventPattern<EventHandler, EventArgs>
            (
                h => CultureManager.Instance.CultureChanged += h,
                h => CultureManager.Instance.CultureChanged -= h
            )
            .Where(_ => !_skipCultureManagerCultureChangedNotification)
            .Subscribe(_ => ChangeCulture(CultureManager.Instance.CurrentCulture))
            .DisposeWith(Disposables);
    }

    #region Public methods
    /// <inheritdoc/>
    public void ChangeCulture(CultureInfo newCulture)
    {
        var previousCulture = CurrentCulture;
        if (Equals(newCulture, previousCulture))
            return;
        
        Dispatcher.UIThread.Post(() =>
        {
            RemoveLocaleResourcesFromResourceDictionary(CurrentCulture);
            AddLocaleResourcesToResourceDictionary(newCulture);

            SetCurrentCultureAndChangeCultureManagerCulture(newCulture);
            CultureChanged?.Invoke(this, new CultureChangedEventArgs(previousCulture, CurrentCulture));
        });
    }

    /// <inheritdoc/>
    public string GetLocalizedString(string key)
    {
        key = $"{_stringResourceKeyPrefix}{key}";
        
        if (!_localeResourceDictionary.TryGetResourceOnUiThread(key, out var resource))
            throw new InvalidOperationException($"The localized string with the key '{key}' was not found.");
        
        return (resource as string)!;
    }
    #endregion

    #region Private methods
    private CultureInfo GetInitialCulture(LocalizationOptions options)
    {
        if (options.SetInitialCultureToDefault)
            return options.DefaultCulture;
        
        var currentCulture = CultureManager.Instance.CurrentCulture;
        return _localeResourceProvider.AvailableCultures.Contains(currentCulture)
            ? currentCulture
            : options.DefaultCulture;
    }

    private void SetCurrentCultureAndChangeCultureManagerCulture(CultureInfo culture)
    {
        CurrentCulture = culture;
        
        _skipCultureManagerCultureChangedNotification = true;
        CultureManager.Instance.ChangeCulture(culture);
        _skipCultureManagerCultureChangedNotification = false;
    }
    
    private void RemoveLocaleResourcesFromResourceDictionary(CultureInfo culture)
    {
        var localeResources = _localeResourceProvider.GetLocaleResourcesByCulture(culture);
        foreach (var localeResource in localeResources)
            _localeResourceDictionary.MergedDictionaries.Remove(localeResource);
    }
    
    private void AddLocaleResourcesToResourceDictionary(CultureInfo culture)
    {
        var localeResources = _localeResourceProvider.GetLocaleResourcesByCulture(culture);
        foreach (var localeResource in localeResources)
            _localeResourceDictionary.MergedDictionaries.Add(localeResource);
    }
    #endregion
}