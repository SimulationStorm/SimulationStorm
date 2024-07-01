using System;
using System.Collections.Generic;

namespace SimulationStorm.AppSaves;

public class AppSave
{
    public int Id { get; set; }

    public DateTime DateAndTime { get; set; }

    public string Name { get; set; } = null!;

    public virtual List<ServiceSave> ServiceSaves { get; set; } = [];
}