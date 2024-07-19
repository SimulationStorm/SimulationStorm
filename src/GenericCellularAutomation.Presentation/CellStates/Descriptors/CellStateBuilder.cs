using GenericCellularAutomation.Presentation.Common;
using SimulationStorm.Graphics;

namespace GenericCellularAutomation.Presentation.CellStates.Descriptors;

public sealed class CellStateDescriptorBuilder : IFluentBuilder<CellStateDescriptor>
{
    #region Fields
    private string _name = string.Empty;

    private Color _color;
    #endregion

    #region Methods
    public CellStateDescriptorBuilder HasName(string name)
    {
        _name = name;
        return this;
    }

    public CellStateDescriptorBuilder HasColor(Color color)
    {
        _color = color;
        return this;
    }

    public CellStateDescriptor Build() => new(_name, _color);
    #endregion
}