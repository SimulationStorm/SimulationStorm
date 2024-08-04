using SimulationStorm.Simulation.History.Presentation.Services;

namespace GenericCellularAutomation.Presentation.Management;

public sealed class GcaSaveManager(GcaManager gcaManager) : SimulationSaveManagerBase<GcaSave>(gcaManager);