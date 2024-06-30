using System.Diagnostics;
using Microsoft.Extensions.Logging;
using SimulationStorm.Exceptions.Unhandled;

namespace SimulationStorm.Exceptions.Logging;

public class UnhandledExceptionLogger(ILogger<UnhandledExceptionLogger> logger) : IHandleUnhandledException
{
    public void HandleUnhandledException(object sender, UnhandledExceptionEventArgs e) =>
        logger.LogCritical(e.Exception.Demystify(), "Unhandled exception caught by {sender}", sender);
}