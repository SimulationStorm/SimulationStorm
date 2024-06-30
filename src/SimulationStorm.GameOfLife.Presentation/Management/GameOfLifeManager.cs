using System.Collections.Generic;
using System.Threading.Tasks;
using SimulationStorm.GameOfLife.Algorithms;
using SimulationStorm.GameOfLife.DataTypes;
using SimulationStorm.GameOfLife.Presentation.Commands;
using SimulationStorm.Primitives;
using SimulationStorm.Simulation.Bounded.Presentation.Commands;
using SimulationStorm.Simulation.Bounded.Presentation.Services;
using SimulationStorm.Simulation.CellularAutomation;
using SimulationStorm.Simulation.CellularAutomation.Presentation.Commands;
using SimulationStorm.Simulation.CellularAutomation.Presentation.Services;
using SimulationStorm.Simulation.History.Presentation.Commands;
using SimulationStorm.Simulation.History.Presentation.Services;
using SimulationStorm.Simulation.Presentation.SimulationManager;
using SimulationStorm.Simulation.Resetting.Presentation.Commands;
using SimulationStorm.Simulation.Resetting.Presentation.Services;
using SimulationStorm.Simulation.Running.Presentation.Commands;
using SimulationStorm.Simulation.Running.Presentation.Services;
using SimulationStorm.Simulation.Statistics.Presentation.SummaryStats;
using SimulationStorm.Utilities.Benchmarking;

namespace SimulationStorm.GameOfLife.Presentation.Management;

public class GameOfLifeManager :
    SimulationManagerBase,
    ISaveableSimulationManager<GameOfLifeSave>,
    ISummarizableSimulationManager<GameOfLifeSummary>,
    IAdvanceableSimulationManager,
    IBoundedSimulationManager,
    IResettableSimulationManager,
    IBoundedCellularAutomationManager<GameOfLifeCellState>
{
    #region Properties
    public Size WorldSize { get; private set; }

    public WorldWrapping WorldWrapping { get; private set; }

    public GameOfLifeAlgorithm Algorithm { get; private set; }

    public GameOfLifeRule Rule { get; private set; } = null!;
    #endregion
    
    #region Fields
    private readonly IGameOfLifeFactory _gameOfLifeFactory;

    private IGameOfLife _gameOfLifeImpl;
    #endregion
    
    public GameOfLifeManager
    (
        IBenchmarkingService benchmarkingService,
        IGameOfLifeFactory gameOfLifeFactory,
        GameOfLifeManagerOptions options
    )
        : base(benchmarkingService, options)
    {
        _gameOfLifeFactory = gameOfLifeFactory;

        _gameOfLifeImpl = _gameOfLifeFactory.CreateGameOfLife(options.Algorithm,
            options.WorldSize, options.WorldWrapping, options.Rule);

        Algorithm = options.Algorithm;
        
        RememberGameOfLifePropertyValues();
    }

    #region Reading methods
    public Task<GameOfLifeSave> SaveAsync() =>
        WithReadLockAsync(_gameOfLifeImpl.Save);
    
    public Task<BenchmarkResult<GameOfLifeSave>> SaveAndMeasureAsync() =>
        MeasureWithReadLockAsync(_gameOfLifeImpl.Save);

    public Task<GameOfLifeSummary> SummarizeAsync() =>
        WithReadLockAsync(_gameOfLifeImpl.Summarize);
    
    public Task<BenchmarkResult<GameOfLifeSummary>> SummarizeAndMeasureAsync() =>
        MeasureWithReadLockAsync(_gameOfLifeImpl.Summarize);

    public Task<IEnumerable<Point>> GetAliveCellsAsync() =>
        WithReadLockAsync(_gameOfLifeImpl.GetAliveCells);
    #endregion

    #region Writing methods
    public Task ChangeWorldSizeAsync(Size newSize) =>
        QueueCommandExecutionAsync(new ChangeWorldSizeCommand(newSize));

    public Task ChangeWorldWrappingAsync(WorldWrapping newWrapping) =>
        QueueCommandExecutionAsync(new ChangeWorldWrappingCommand(newWrapping));

    public Task ChangeRuleAsync(GameOfLifeRule newRule) =>
        QueueCommandExecutionAsync(new ChangeRuleCommand(newRule));

    public Task ChangeAlgorithmAsync(GameOfLifeAlgorithm newAlgorithm) =>
        QueueCommandExecutionAsync(new ChangeAlgorithmCommand(newAlgorithm));

    public Task RestoreStateAsync(GameOfLifeSave save, bool isRestoringFromAppState = false) =>
        QueueCommandExecutionAsync(new RestoreStateCommand(save, isRestoringFromAppState));
    
    public Task AdvanceAsync() =>
        QueueCommandExecutionAsync(new AdvanceCommand());

    public Task ResetAsync() =>
        QueueCommandExecutionAsync(new ResetCommand());

    public Task ChangeCellStatesAsync(IEnumerable<Point> cells, GameOfLifeCellState newState) =>
        QueueCommandExecutionAsync(new DrawCommand<GameOfLifeCellState>(cells, newState));

    public Task PlacePatternAtPositionsAsync(GameOfLifePattern pattern, IEnumerable<Point> positions, bool placeWithOverlay) =>
        QueueCommandExecutionAsync(new PlacePatternCommand(pattern, positions, placeWithOverlay));

    public Task PopulateRandomlyAsync(double cellBirthProbability) =>
        QueueCommandExecutionAsync(new PopulateRandomlyCommand(cellBirthProbability));
    #endregion

    protected override void ExecuteCommand(SimulationCommand command)
    {
        switch (command)
        {
            case ChangeWorldSizeCommand changeWorldSizeCommand:
            {
                ExecuteChangeWorldSize(changeWorldSizeCommand);
                break;
            }
            case ChangeAlgorithmCommand changeAlgorithmCommand:
            {
                ExecuteChangeAlgorithm(changeAlgorithmCommand);
                break;
            }
            case ChangeRuleCommand changeRuleCommand:
            {
                ExecuteChangeRule(changeRuleCommand);
                break;
            }
            case ChangeWorldWrappingCommand changeWorldWrappingCommand:
            {
                ExecuteChangeWorldWrapping(changeWorldWrappingCommand);
                break;
            }
            case AdvanceCommand advanceCommand:
            {
                ExecuteAdvance(advanceCommand);
                break;
            }
            case ResetCommand resetCommand:
            {
                ExecuteReset(resetCommand);
                break;
            }
            case RestoreStateCommand restoreStateCommand:
            {
                ExecuteRestoreState(restoreStateCommand);
                break;
            }
            case DrawCommand<GameOfLifeCellState> drawCommand:
            {
                ExecuteDraw(drawCommand);
                break;
            }
            case PlacePatternCommand placePatternCommand:
            {
                ExecutePlacePattern(placePatternCommand);
                break;
            }
            case PopulateRandomlyCommand populateRandomlyCommand:
            {
                ExecutePopulateRandomly(populateRandomlyCommand);
                break;
            }
        }
    }

    #region Commands execution
    private void ExecuteChangeWorldSize(ChangeWorldSizeCommand command)
    {
        _gameOfLifeImpl.ChangeWorldSize(command.NewSize);
        WorldSize = _gameOfLifeImpl.WorldSize;
    }

    private void ExecuteChangeAlgorithm(ChangeAlgorithmCommand command)
    {
        var newAlgorithm = command.NewAlgorithm;
            
        var newImplementation = _gameOfLifeFactory.CreateGameOfLife(newAlgorithm,
            _gameOfLifeImpl.WorldSize, _gameOfLifeImpl.WorldWrapping, _gameOfLifeImpl.Rule);

        foreach (var cell in _gameOfLifeImpl.GetAliveCells())
            newImplementation.SetCellState(cell, GameOfLifeCellState.Alive);

        _gameOfLifeImpl = newImplementation;

        Algorithm = newAlgorithm;
    }

    private void ExecuteChangeRule(ChangeRuleCommand command)
    {
        _gameOfLifeImpl.Rule = command.NewRule;
        Rule = _gameOfLifeImpl.Rule;
    }

    private void ExecuteChangeWorldWrapping(ChangeWorldWrappingCommand command)
    {
        _gameOfLifeImpl.WorldWrapping = command.NewWorldWrapping;
        WorldWrapping = _gameOfLifeImpl.WorldWrapping;
    }

    private void ExecuteAdvance(AdvanceCommand command) =>
        _gameOfLifeImpl.Advance();

    private void ExecuteReset(ResetCommand command) =>
        _gameOfLifeImpl.Reset();

    private void ExecuteRestoreState(RestoreStateCommand command)
    {
        var state = (GameOfLifeSave)command.State;
        switch (state.Algorithm)
        {
            case GameOfLifeAlgorithm.Bitwise when Algorithm is not GameOfLifeAlgorithm.Bitwise:
            {
                ExecuteChangeAlgorithm(new ChangeAlgorithmCommand(GameOfLifeAlgorithm.Bitwise));
                break;
            }
            case GameOfLifeAlgorithm.Smart when Algorithm is not GameOfLifeAlgorithm.Smart:
            {
                ExecuteChangeAlgorithm(new ChangeAlgorithmCommand(GameOfLifeAlgorithm.Smart));
                break;
            }
        }
        
        _gameOfLifeImpl.RestoreState(state);
        RememberGameOfLifePropertyValues();
    }

    private void ExecuteDraw(DrawCommand<GameOfLifeCellState> command)
    {
        var cells = command.Cells;
        var newState = command.NewState;
            
        foreach (var cell in cells)
            _gameOfLifeImpl.SetCellState(cell, newState);
    }

    private void ExecutePlacePattern(PlacePatternCommand command)
    {
        var pattern = command.Pattern;
        var positions = command.Positions;
            
        foreach (var position in positions)
            _gameOfLifeImpl.PlacePattern(pattern, position, command.PlaceWithOverlay);
    }

    private void ExecutePopulateRandomly(PopulateRandomlyCommand command) =>
        _gameOfLifeImpl.PopulateRandomly(command.CellBirthProbability);
    #endregion

    private void RememberGameOfLifePropertyValues()
    {
        WorldSize = _gameOfLifeImpl.WorldSize;
        WorldWrapping = _gameOfLifeImpl.WorldWrapping;
        Rule = _gameOfLifeImpl.Rule;        
    }
}