﻿using System.Collections.Generic;
using DotNext.Collections.Generic;
using GenericCellularAutomation.Presentation.Common;

namespace GenericCellularAutomation.Presentation.Rules.Descriptors;

public sealed class RuleSetDescriptorBuilder : IFluentBuilder<RuleSetDescriptor>
{
    #region Fields
    private string _name = string.Empty;

    private int _repetitionCount;

    private readonly IList<RuleDescriptor> _rules = new List<RuleDescriptor>();
    #endregion

    #region Methods
    public RuleSetDescriptorBuilder HasName(string name)
    {
        _name = name;
        return this;
    }
    
    public RuleSetDescriptorBuilder HasRepetitionCount(int repetitionCount)
    {
        _repetitionCount = repetitionCount;
        return this;
    }

    public RuleSetDescriptorBuilder HasRules(params RuleDescriptor[] rules)
    {
        _rules.AddAll(rules);
        return this;
    }

    public RuleSetDescriptor Build() => new(_name, _repetitionCount, (IReadOnlyList<RuleDescriptor>)_rules);
    #endregion
}