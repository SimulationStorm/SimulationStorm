using System.Collections.Generic;
using System.Linq;
using GenericCellularAutomation.Presentation.CellStates.Descriptors;
using GenericCellularAutomation.Presentation.Common;

namespace GenericCellularAutomation.Presentation.Patterns;

public sealed class PatternDescriptorBuilder : IFluentBuilder<PatternDescriptor>
{
    #region Fields
    private string _name = string.Empty;

    private string _scheme = string.Empty;

    private IReadOnlyDictionary<string, CellStateDescriptor> _cellStateByNames = null!;
    #endregion

    #region Methods
    public PatternDescriptorBuilder HasName(string name)
    {
        _name = name;
        return this;
    }

    public PatternDescriptorBuilder HasScheme(string scheme)
    {
        _scheme = scheme;
        return this;
    }

    public PatternDescriptorBuilder HasSchemeCellStateNames(
        IReadOnlyDictionary<string, CellStateDescriptor> cellStateByNames)
    {
        _cellStateByNames = cellStateByNames;
        return this;
    }
    
    public PatternDescriptor Build() => new
    (
        _name,
        Pattern.FromScheme
        (
            _scheme,
            _cellStateByNames.ToDictionary(kv => kv.Key, kv => kv.Value.CellState)
        )
    );
    #endregion
}