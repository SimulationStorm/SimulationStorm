using System;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Threading;
using SimulationStorm.Dialogs.Presentation;

namespace SimulationStorm.Dialogs.Avalonia;

[TemplatePart(partBackgroundControl, typeof(Control))]
[TemplatePart(partDialogContentControl, typeof(ContentControl))]
[PseudoClasses(pcDialogShown)]
public class DialogManager : TemplatedControl, IDialogManager
{
    #region Constants
    private const string partBackgroundControl = "PART_BackgroundControl";
    private const string partDialogContentControl = "PART_DialogContentControl";
    
    private const string pcDialogShown = ":dialog-shown";
    #endregion
    
    #region Fields
    private Control _backgroundControl = null!;

    private ContentControl _dialogContentControl = null!;
    
    private IDialog? _currentDialog;
    #endregion
    
    #region Event handlers
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _backgroundControl = e.NameScope.Find<Control>(partBackgroundControl)!;
        _backgroundControl.PointerPressed += OnBackgroundControlPointerPressed;
        
        _dialogContentControl = e.NameScope.Find<ContentControl>(partDialogContentControl)!;
    }

    private void OnBackgroundControlPointerPressed(object? _, PointerPressedEventArgs __)
    {
        if (_currentDialog?.IsBackgroundClosingAllowed is true)
            CloseCurrentDialog();
    }

    private void OnCurrentDialogCloseRequested(object? _, EventArgs __) => Dispatcher.UIThread.Post(CloseCurrentDialog);
    #endregion
    
    public void ShowDialog(IDialog dialog)
    {
        dialog.CloseRequested += OnCurrentDialogCloseRequested;
        
        Dispatcher.UIThread.Post(() =>
        {
            _currentDialog = dialog;
            _dialogContentControl.Content = dialog;
            
            UpdatePseudoClasses();
        });
    }

    #region Helpers
    private void CloseCurrentDialog()
    {
        _currentDialog = null;
        _dialogContentControl.Content = null;
            
        UpdatePseudoClasses();
    }

    private void UpdatePseudoClasses() => PseudoClasses.Set(pcDialogShown, _currentDialog is not null);
    #endregion
}