using System;

namespace SimulationStorm.Utilities;

public class IntervalActionExecutor : IIntervalActionExecutor
{
    private const int MinimumCounterValue = 0;
    
    #region Properties
    public bool IsEnabled
    {
        get => _isEnabled;
        set
        {
            _isEnabled = value;
            
            if (!_isEnabled)
                ResetCounter();
        }
    }
    
    public int Interval
    {
        get => _interval;
        set
        {
            ValidateInterval(value);
            
            _interval = value;
            
            _counter = Math.Clamp(_counter, MinimumCounterValue, _interval);
        }
    }
    
    public bool IsExecutionNeeded => IsEnabled && _counter == Interval;
    #endregion

    #region Fields
    private bool _isEnabled = true;
    
    private int _interval;

    private int _counter = MinimumCounterValue;
    #endregion

    #region Public methods
    public bool GetIsExecutionNeededAndMoveNext()
    {
        var isExecutionNeeded = IsExecutionNeeded;
        MoveNext();
        return isExecutionNeeded;
    }
    
    public void MoveNext()
    {
        if (!IsEnabled)
            return;

        _counter++;

        if (_counter > Interval)
            ResetCounter();
    }
    #endregion

    #region Private methods
    private static void ValidateInterval(int interval) =>
        ArgumentOutOfRangeException.ThrowIfNegative(interval, nameof(interval));

    private void ResetCounter() => _counter = MinimumCounterValue;
    #endregion
}