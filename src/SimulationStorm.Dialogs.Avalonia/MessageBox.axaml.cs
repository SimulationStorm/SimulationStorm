using Avalonia;
using Avalonia.Interactivity;
using SimulationStorm.Avalonia.Controls;

namespace SimulationStorm.Dialogs.Avalonia;

public partial class MessageBox : WindowExtended
{
    public static readonly StyledProperty<string?> MessageProperty =
        AvaloniaProperty.Register<MessageBox, string?>(nameof(Message));
    
    public string? Message
    {
        get => GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }
    
    public MessageBox() => InitializeComponent();

    private void OnOkButtonClick(object? _, RoutedEventArgs __) => Close();
}