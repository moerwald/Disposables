using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Disposables
{
    public sealed class CompositeDisposable : IEnumerable<IDisposable>, IDisposable
    {
        private readonly List<IDisposable> _disposables;
        private bool _disposed;

        #region CTOR
        public CompositeDisposable()
            : this(new IDisposable[] { })
        { }

        public CompositeDisposable(IEnumerable<IDisposable> disposables)
        {
            if (disposables == null) { throw new ArgumentNullException(nameof(disposables)); }
            this._disposables = new List<IDisposable>(disposables);
        }

        public CompositeDisposable(params IDisposable[] disposables)
        {
            if (disposables == null) { throw new ArgumentNullException(nameof(disposables)); }
            this._disposables = new List<IDisposable>(disposables);
        }

        #endregion

        #region Add disposables
        public void Add(IDisposable disposable)
        {
            if (disposable == null) { throw new ArgumentNullException(nameof(disposable)); }
            lock (_disposables)
            {
                if (_disposed)
                {
                    disposable.Dispose();
                }
                else
                {
                    _disposables.Add(disposable);
                }
            }
        }

        public IDisposable Add(Action action)
        {
            if (action == null) { throw new ArgumentNullException(nameof(action)); }

            var disposable = new Simple(action);
            this.Add(disposable);
            return disposable;
        }

        #endregion

        public void Clear()
        {
            lock (_disposables)
            {
                var disposables = _disposables.ToArray();
                _disposables.Clear();

                foreach (var disp in disposables)
                {
                    disp.Dispose();
                }
            }
        }

        public void Dispose()
        {
            lock (_disposables)
            {
                if (!_disposed)
                {
                    this.Clear();
                }
                _disposed = true;
            }
        }

        public IEnumerator<IDisposable> GetEnumerator()
        {
            lock (_disposables)
            {
                return _disposables.ToArray().AsEnumerable().GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public bool IsDisposed => _disposed;
    }
}
