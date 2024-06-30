using System;
using CommunityToolkit.Mvvm.Input;
using SimulationStorm.Utilities.Disposing;

namespace SimulationStorm.Dialogs.Presentation;

public abstract class DialogViewModelBase : DisposableObservableObject, IDialog, IDialogViewModel
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

    protected void Close() => CloseRequested?.Invoke(this, EventArgs.Empty);
}