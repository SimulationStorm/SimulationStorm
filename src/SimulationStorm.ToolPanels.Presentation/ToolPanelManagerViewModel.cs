using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimulationStorm.Threading.Presentation;
using SimulationStorm.Utilities.Disposing;

namespace SimulationStorm.ToolPanels.Presentation;

public partial class ToolPanelManagerViewModel : DisposableObservableObject
{
    #region Properties
    public IEnumerable<ToolPanel> OpenedToolPanels => GetOpenedToolPanels();
    
    public IReadOnlyCollection<ToolPanel> TopLeftToolPanels =>
        GetToolPanelsAtPosition(ToolPanelPosition.TopLeft);
    public IReadOnlyCollection<ToolPanel> TopRightToolPanels =>
        GetToolPanelsAtPosition(ToolPanelPosition.TopRight);
    public IReadOnlyCollection<ToolPanel> BottomLeftToolPanels =>
        GetToolPanelsAtPosition(ToolPanelPosition.BottomLeft);
    public IReadOnlyCollection<ToolPanel> BottomRightToolPanels =>
        GetToolPanelsAtPosition(ToolPanelPosition.BottomRight);

    public ToolPanel? TopLeftToolPanel =>
        _toolPanelManager.GetOpenedToolPanelAtPosition(ToolPanelPosition.TopLeft);
    public ToolPanel? TopRightToolPanel =>
        _toolPanelManager.GetOpenedToolPanelAtPosition(ToolPanelPosition.TopRight);
    public ToolPanel? BottomLeftToolPanel =>
        _toolPanelManager.GetOpenedToolPanelAtPosition(ToolPanelPosition.BottomLeft);
    public ToolPanel? BottomRightToolPanel =>
        _toolPanelManager.GetOpenedToolPanelAtPosition(ToolPanelPosition.BottomRight);

    [NotifyPropertyChangedFor(nameof(OpenedToolPanels))]
    [NotifyPropertyChangedFor(nameof(TopLeftToolPanel))]
    [ObservableProperty]
    private object? _topLeftToolPanelViewModel;
    
    [NotifyPropertyChangedFor(nameof(OpenedToolPanels))]
    [NotifyPropertyChangedFor(nameof(TopRightToolPanel))]
    [ObservableProperty]
    private object? _topRightToolPanelViewModel;

    [NotifyPropertyChangedFor(nameof(OpenedToolPanels))]
    [NotifyPropertyChangedFor(nameof(BottomLeftToolPanel))]
    [ObservableProperty]
    private object? _bottomLeftToolPanelViewModel;

    [NotifyPropertyChangedFor(nameof(OpenedToolPanels))]
    [NotifyPropertyChangedFor(nameof(BottomRightToolPanel))]
    [ObservableProperty]
    private object? _bottomRightToolPanelViewModel;
    #endregion

    #region Commands
    [RelayCommand]
    private void ToggleToolPanel(ToolPanel toolPanel) => _toolPanelManager.ToggleToolPanelVisibility(toolPanel);

    [RelayCommand]
    private void OpenToolPanel(ToolPanel toolPanel) => _toolPanelManager.OpenToolPanel(toolPanel);
    
    [RelayCommand]
    private void CloseToolPanel(ToolPanel toolPanel) => _toolPanelManager.CloseToolPanel(toolPanel);
    #endregion

    #region Fields
    private readonly IToolPanelManager _toolPanelManager;

    private readonly IToolPanelViewModelFactory _toolPanelViewModelFactory;
    #endregion

    public ToolPanelManagerViewModel
    (
        IUiThreadScheduler uiThreadScheduler,
        IToolPanelManager toolPanelManager,
        IToolPanelViewModelFactory toolPanelViewModelFactory)
    {
        _toolPanelManager = toolPanelManager;
        _toolPanelViewModelFactory = toolPanelViewModelFactory;
        
        Observable
            .FromEventPattern<EventHandler<ToolPanelAtPositionChangedEventArgs>, ToolPanelAtPositionChangedEventArgs>
            (
                h => _toolPanelManager.ToolPanelAtPositionChanged += h,
                h => _toolPanelManager.ToolPanelAtPositionChanged -= h
            )
            .ObserveOn(uiThreadScheduler)
            .Select(e => e.EventArgs)
            .Subscribe(OnToolPanelAtPositionChanged)
            .DisposeWith(Disposables);
        
        UpdateTopLeftToolPanelViewModel(_toolPanelManager.GetOpenedToolPanelAtPosition(ToolPanelPosition.TopLeft));
        UpdateTopRightToolPanelViewModel(_toolPanelManager.GetOpenedToolPanelAtPosition(ToolPanelPosition.TopRight));
        UpdateBottomLeftToolPanelViewModel(_toolPanelManager.GetOpenedToolPanelAtPosition(ToolPanelPosition.BottomLeft));
        UpdateBottomRightToolPanelViewModel(_toolPanelManager.GetOpenedToolPanelAtPosition(ToolPanelPosition.BottomRight));
    }

    #region Private methods

    private void OnToolPanelAtPositionChanged(ToolPanelAtPositionChangedEventArgs e)
    {
        if (e.Position is ToolPanelPosition.TopLeft)
            UpdateTopLeftToolPanelViewModel(e.NewVisibleToolPanel);
        else if (e.Position is ToolPanelPosition.TopRight)
            UpdateTopRightToolPanelViewModel(e.NewVisibleToolPanel);
        else if (e.Position is ToolPanelPosition.BottomLeft)
            UpdateBottomLeftToolPanelViewModel(e.NewVisibleToolPanel);
        else if (e.Position is ToolPanelPosition.BottomRight)
            UpdateBottomRightToolPanelViewModel(e.NewVisibleToolPanel);
    }
    
    private IEnumerable<ToolPanel> GetOpenedToolPanels()
    {
        var openedToolPanels = new List<ToolPanel>(4);
            
        if (TopLeftToolPanel is not null)
            openedToolPanels.Add(TopLeftToolPanel);
        if (TopRightToolPanel is not null)
            openedToolPanels.Add(TopRightToolPanel);
        if (BottomLeftToolPanel is not null)
            openedToolPanels.Add(BottomLeftToolPanel);
        if (BottomRightToolPanel is not null)
            openedToolPanels.Add(BottomRightToolPanel);
            
        return openedToolPanels;
    }
    
    private IReadOnlyCollection<ToolPanel> GetToolPanelsAtPosition(ToolPanelPosition position) =>
        _toolPanelManager.ToolPanelPositions
            .Where(x => x.Value == position)
            .Select(x => x.Key)
            .ToArray();
    
    private void UpdateTopLeftToolPanelViewModel(ToolPanel? toolPanel) =>
        TopLeftToolPanelViewModel = toolPanel is null ? null : _toolPanelViewModelFactory.Create(toolPanel);
    
    private void UpdateTopRightToolPanelViewModel(ToolPanel? toolPanel) =>
        TopRightToolPanelViewModel = toolPanel is null ? null : _toolPanelViewModelFactory.Create(toolPanel);
    
    private void UpdateBottomLeftToolPanelViewModel(ToolPanel? toolPanel) =>
        BottomLeftToolPanelViewModel = toolPanel is null ? null : _toolPanelViewModelFactory.Create(toolPanel);

    private void UpdateBottomRightToolPanelViewModel(ToolPanel? toolPanel) =>
        BottomRightToolPanelViewModel = toolPanel is null ? null : _toolPanelViewModelFactory.Create(toolPanel);
    #endregion
}