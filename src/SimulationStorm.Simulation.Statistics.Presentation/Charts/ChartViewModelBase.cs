using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using SimulationStorm.Collections.Presentation;
using SimulationStorm.Collections.Universal;
using SimulationStorm.Localization.Presentation;
using SimulationStorm.Threading.Presentation;
using SimulationStorm.Utilities.Disposing;
using SimulationStorm.Utilities.Indexing;

namespace SimulationStorm.Simulation.Statistics.Presentation.Charts;

public abstract class ChartViewModelBase<TRecord, TRecordModel> :
    DisposableObject
    where TRecord : notnull
    where TRecordModel : class, IIndexedObject
{
    public ReadOnlyObservableCollection<TRecordModel> RecordModels => _recordModels;

    protected ILocalizationManager LocalizationManager { get; set; }
    
    private ReadOnlyObservableCollection<TRecordModel> _recordModels = null!;
    
    protected ChartViewModelBase
    (
        IUiThreadScheduler uiThreadScheduler,
        ICollectionManager<TRecord> collectionManager,
        ILocalizationManager localizationManager)
    {
        LocalizationManager = localizationManager;
        
        collectionManager.Collection
            .IndexItemsAndBind<IUniversalCollection<TRecord>, TRecord, TRecordModel>
            (
                CreateRecordModel,
                out _recordModels,
                scheduler: uiThreadScheduler
            )
            .DisposeWith(Disposables);
        
        Observable
            .FromEventPattern<EventHandler<CultureChangedEventArgs>, CultureChangedEventArgs>
            (
                h => LocalizationManager.CultureChanged += h,
                h => LocalizationManager.CultureChanged -= h
            )
            .Subscribe(_ => OnCultureChanged())
            .DisposeWith(Disposables);
    }

    protected virtual void OnCultureChanged() { }
    
    #region Protected methods
    protected abstract TRecordModel CreateRecordModel(TRecord record);

    protected string GetLocalizedString(string key) => LocalizationManager.GetLocalizedString(key);
    #endregion
}