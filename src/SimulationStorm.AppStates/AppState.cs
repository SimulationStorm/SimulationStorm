using System;
using System.Collections.Generic;

namespace SimulationStorm.AppStates;

public class AppState
{
    public int Id { get; set; }

    public DateTime DateAndTime { get; set; }

    public string Name { get; set; } = null!;

    public virtual List<AppServiceState> ServiceStates { get; set; } = [];
}