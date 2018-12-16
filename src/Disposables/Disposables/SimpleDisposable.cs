using System;

namespace Disposables
{
    /// <summary>
    /// Wrapes the given action in the disposable pattern.
    /// </summary>
    public sealed class SimpleDisposable : IIsDisposed
    {
        private Action _action;

        public static SimpleDisposable Create(Action action) => new SimpleDisposable(action);

        public SimpleDisposable(Action action) => _action = action;

        public void Dispose()
        {
            if (!IsDisposed)
            {
                System.Threading.Interlocked.Exchange(ref _action, null)?.Invoke();
            }
        }

        public bool IsDisposed => _action != null;
    }
}
