using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CommunityToolkit.Mvvm.Input;
using DynamicData.Binding;
using SimulationStorm.GameOfLife.Presentation.Drawing;
using SimulationStorm.Threading.Presentation;
using SimulationStorm.Utilities.Disposing;

namespace SimulationStorm.GameOfLife.Presentation.Patterns;

public partial class PatternsViewModel : DisposableObservableObject
{
    #region Properties
    public NamedPattern? CurrentPattern
    {
        get => _drawingSettings.CurrentPattern;
        set => _drawingSettings.CurrentPattern = value;
    }

    public bool PlacePatternWithOverlay
    {
        get => _drawingSettings.PlacePatternWithOverlay;
        set => _drawingSettings.PlacePatternWithOverlay = value;
    }
    #endregion
    
    [RelayCommand(CanExecute = nameof(CanUnselect))]
    private void Unselect() => CurrentPattern = null;
    private bool CanUnselect() => CurrentPattern is not null;
    
    private readonly GameOfLifeDrawingSettings _drawingSettings;

    public PatternsViewModel(IUiThreadScheduler uiThreadScheduler, GameOfLifeDrawingSettings drawingSettings)
    {
        _drawingSettings = drawingSettings;
        
        _drawingSettings
            .WhenValueChanged(x => x.CurrentPattern, false)
            .ObserveOn(uiThreadScheduler)
            .Subscribe(_ =>
            {
                OnPropertyChanged(nameof(CurrentPattern));
                UnselectCommand.NotifyCanExecuteChanged();
            })
            .DisposeWith(Disposables);
        
        _drawingSettings
            .WhenValueChanged(x => x.PlacePatternWithOverlay, false)
            .ObserveOn(uiThreadScheduler)
            .Subscribe(_ => OnPropertyChanged(nameof(PlacePatternWithOverlay)))
            .DisposeWith(Disposables);
    }
}