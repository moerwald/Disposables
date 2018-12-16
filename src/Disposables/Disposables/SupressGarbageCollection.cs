using System;
using System.Threading;

namespace Disposables
{
    /// <summary>
    /// Use this in case of your parent object has implemented finalizers.
    /// </summary>
    public class SupressGarbageCollection : IIsDisposed
    {
        private readonly object _parent;
        private SimpleDisposable _simple;

        public SupressGarbageCollection(Action action, object parentObject)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            _simple = new SimpleDisposable(action);
            this._parent = parentObject ?? throw new ArgumentNullException(nameof(parentObject));
        }

        public bool IsDisposed => _simple != null;

        public void Dispose()
        {
            if (_simple != null)
            {
                Interlocked.Exchange(ref _simple, null)?.Dispose();
                GC.SuppressFinalize(_parent);
            }
        }
    }
}
