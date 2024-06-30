using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using SimulationStorm.Presentation.Navigation;

namespace SimulationStorm.Avalonia.Navigation;

[TemplatePart(partContentControl, typeof(ContentControl))]
public class NavigationManager : TemplatedControl, INavigationManager
{
    private const string partContentControl = "PART_ContentControl";

    public object? CurrentContent => _contentHistory.TryPeek(out var content) ? content : null;

    public event EventHandler<NavigationContentChangedEventArgs>? ContentChanged;

    #region Fields
    private ContentControl? _contentControl;
    
    private readonly Stack<object> _contentHistory = new();
    #endregion

    #region Public methods
    public void Navigate(object content)
    {
        var previousContent = CurrentContent;
        _contentHistory.Push(content);
        
        UpdateContentControlContent();
        NotifyContentChanged(previousContent);
    }

    public bool CanNavigateToPreviousContent() => _contentHistory.Count > 1;

    public void NavigateToPreviousContent()
    {
        if (!CanNavigateToPreviousContent())
            throw new InvalidOperationException();

        var previousContent = CurrentContent;
        _contentHistory.Pop();
        
        UpdateContentControlContent();
        NotifyContentChanged(previousContent);
    }
    #endregion

    #region Event handlers
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _contentControl = e.NameScope.Find<ContentControl>(partContentControl)!;
        UpdateContentControlContent();
    }
    #endregion

    #region Private methods
    private void UpdateContentControlContent()
    {
        if (_contentControl is not null)
            _contentControl.Content = CurrentContent;
    }

    private void NotifyContentChanged(object? previousContent) =>
        ContentChanged?.Invoke(this, new NavigationContentChangedEventArgs(previousContent, CurrentContent!));
    #endregion
}