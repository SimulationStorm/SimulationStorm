using System;

namespace SimulationStorm.AppSaves.Entities;

public class ServiceSave
{
    public int Id { get; set; }
    
    public int AppSaveId { get; set; }
    
    public virtual AppSave AppSave { get; set; } = null!;
    
    public Type SaveObjectType { get; set; } = null!;

    public object SaveObject { get; set; } = null!;
}