using System;

namespace Disposables
{
    public class SupressGarbageCollection : IIsDisposed
    {
        private volatile bool _disposed = false;
        private Simple _simple;

        public SupressGarbageCollection(Action action)
        {
            _simple = new Simple(action);
        }

        public bool IsDiposed => _simple != null;

        public void Dispose()
        {
            if (_simple != null)
            {
                System.Threading.Interlocked.Exchange(ref _simple, null)?.Dispose();
                GC.SuppressFinalize(this);
            }
        }
    }
}
