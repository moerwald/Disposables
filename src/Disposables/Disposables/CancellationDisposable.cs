using System;
using System.Threading;

namespace Disposables
{
    /// <summary>
    /// Wraps the Cancel call in Dispose.
    /// </summary>
    public class CancellationDisposable : IIsDisposed
    {
        private readonly CancellationTokenSource _cts;

        public CancellationDisposable(CancellationTokenSource cts) => _cts = cts ?? throw new ArgumentNullException(nameof(cts));

        public CancellationDisposable() : this(new CancellationTokenSource()) { }

        public CancellationToken Token => _cts.Token;

        public void Dispose() => _cts.Cancel();

        public bool IsDisposed => _cts.IsCancellationRequested;
    }
}
