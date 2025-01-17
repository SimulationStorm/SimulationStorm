﻿using System;
using SimulationStorm.AppSaves.Entities;

namespace SimulationStorm.AppSaves;

public interface IAppSavesEntityFactory
{
    AppSave CreateAppSave(string name);
    
    ServiceSave CreateServiceSave(Type saveObjectType, object saveObject);
}