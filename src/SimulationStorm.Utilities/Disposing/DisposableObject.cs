using System.Reactive.Disposables;
using Disposable = DotNext.Disposable;

namespace SimulationStorm.Utilities.Disposing;

public abstract class DisposableObject : Disposable
{
    protected CompositeDisposable Disposables { get; } = new();

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        
        if (disposing)
            Disposables.Dispose();
    }
}