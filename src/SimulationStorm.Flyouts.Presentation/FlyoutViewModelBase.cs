using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SimulationStorm.Flyouts.Presentation;

public abstract class FlyoutViewModelBase : ObservableValidator, IFlyoutViewModel
{
    #region Properties
    public bool IsBackgroundClosingAllowed { get; protected set; } = true;

    private bool _isClosingViaCommandAllowed = true;
    protected bool IsClosingViaCommandAllowed
    {
        get => _isClosingViaCommandAllowed;
        set
        {
            _isClosingViaCommandAllowed = value;
            CloseCommand.NotifyCanExecuteChanged();
        }
    }
    #endregion

    private IRelayCommand? _closeCommand;
    public IRelayCommand CloseCommand => _closeCommand ??= new RelayCommand(Close, () => IsClosingViaCommandAllowed);
    
    public event EventHandler? CloseRequested;

    public virtual void OnClosing() { }

    protected void Close() => CloseRequested?.Invoke(this, EventArgs.Empty);
}