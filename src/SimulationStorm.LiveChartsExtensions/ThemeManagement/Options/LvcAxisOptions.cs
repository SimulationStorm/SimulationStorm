using LiveChartsCore.Drawing;
using SimulationStorm.LiveChartsExtensions.FontManagement;

namespace SimulationStorm.LiveChartsExtensions.ThemeManagement.Options;

public class LvcAxisOptions
{
    public LvcFont NameFont { get; init; }

    public int NameFontSize { get; init; }
    
    public LvcColor NameColor { get; init; }
    
    public LvcFont LabelsFont { get; init; }
    
    public int LabelsFontSize { get; init; }
    
    public LvcColor LabelsColor { get; init; }

    public bool AreSeparatorLinesVisible { get; init; }
    
    public LvcColor SeparatorsColor { get; init; }
}