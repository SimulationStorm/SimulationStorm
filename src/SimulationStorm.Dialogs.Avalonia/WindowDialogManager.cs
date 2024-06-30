using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.VisualTree;

namespace SimulationStorm.Dialogs.Avalonia;

public class WindowDialogManager : DialogManager
{
    public WindowDialogManager(TopLevel host) => InstallFromTopLevel(host);

    #region Installation
    private void InstallFromTopLevel(TopLevel topLevel)
    {
        topLevel.TemplateApplied += OnTopLevelTemplateApplied;
        
        var adornerLayer = topLevel.FindDescendantOfType<VisualLayerManager>()?.AdornerLayer;
        if (adornerLayer is null)
            return;
        
        adornerLayer.Children.Add(this);
        AdornerLayer.SetAdornedElement(this, adornerLayer);
    }
    
    private void OnTopLevelTemplateApplied(object? sender, TemplateAppliedEventArgs _)
    {
        if (Parent is AdornerLayer adornerLayer)
        {
            adornerLayer.Children.Remove(this);
            AdornerLayer.SetAdornedElement(this, null);
        }
            
        // Reinstall dialog manager on template reapplied.
        var topLevel = (TopLevel)sender!;
        topLevel.TemplateApplied -= OnTopLevelTemplateApplied;
        InstallFromTopLevel(topLevel);
    }
    #endregion
}