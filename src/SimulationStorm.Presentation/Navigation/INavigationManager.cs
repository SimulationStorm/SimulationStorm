using System;

namespace SimulationStorm.Presentation.Navigation;

public interface INavigationManager
{
    object? CurrentContent { get; }
    
    event EventHandler<NavigationContentChangedEventArgs>? ContentChanged;
    
    void Navigate(object content);

    bool CanNavigateToPreviousContent();
    
    void NavigateToPreviousContent();
}