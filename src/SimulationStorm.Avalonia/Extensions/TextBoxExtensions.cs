using Avalonia.Controls;

namespace SimulationStorm.Avalonia.Extensions;

public static class TextBoxExtensions
{
    public static void MoveCaretToTextEnd(this TextBox textBox)
    {
        if (textBox.Text is not null && textBox.Text.Length > 0)
            textBox.CaretIndex = textBox.Text.Length;
        else
            textBox.CaretIndex = 0;
    }
}