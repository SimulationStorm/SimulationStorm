using System;
using CommunityToolkit.Mvvm.ComponentModel;
using SimulationStorm.Simulation.Presentation.SimulationManager;
using SimulationStorm.Utilities.Indexing;

namespace SimulationStorm.Simulation.History.Presentation.Models;

public partial class HistoryRecordModel(SimulationCommand executedCommand, TimeSpan savingTime) : IndexedObject
{
    public SimulationCommand ExecutedCommand { get; } = executedCommand;

    public TimeSpan SavingTime { get; } = savingTime;

    [ObservableProperty] private HistoryRecordPosition _position;
}