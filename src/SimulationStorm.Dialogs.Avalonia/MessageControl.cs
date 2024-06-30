using Avalonia;
using Avalonia.Controls;

namespace SimulationStorm.Dialogs.Avalonia;

public class MessageControl : UserControl
{
    #region Avalonia properties
    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<MessageControl, string?>(nameof(Title));

    public static readonly StyledProperty<string?> MessageProperty =
        AvaloniaProperty.Register<MessageControl, string?>(nameof(Message));
    #endregion

    #region Properties
    public string? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string? Message
    {
        get => GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }
    #endregion
}