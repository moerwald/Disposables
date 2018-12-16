using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace Disposables
{
    /// <summary>
    /// Holds a list of dispoables. Via <see cref="Add(Action)"/> or <see cref="Add(IDisposable)"/> you can add
    /// several disposables actions. The dispose all of the added actions simple call <see cref="Clear()"/>.
    /// </summary>
    public sealed class CompositeDisposable : IEnumerable<IDisposable>, IDisposable
    {
        private ImmutableList<IDisposable> _disposables;

        public CompositeDisposable()
            : this(new IDisposable[] { })
        { }

        public CompositeDisposable(IEnumerable<IDisposable> disposables)
        {
            if (disposables == null)
            {
                throw new ArgumentNullException(nameof(disposables));
            }

            _disposables = ImmutableList.CreateRange(disposables);
        }

        public CompositeDisposable(params IDisposable[] disposables)
        {
            if (disposables == null) { throw new ArgumentNullException(nameof(disposables)); }
            _disposables = ImmutableList.Create(disposables);
        }

        public void Add(IDisposable disposable)
        {
            if (disposable == null) { throw new ArgumentNullException(nameof(disposable)); }
            if (Disposed)
            {
                disposable.Dispose();
            }
            else
            {
                Interlocked.Exchange(ref _disposables, _disposables.Add(disposable));
            }
        }

        /// <summary>
        /// Takes an Action and wraps it in an IDisposable. The IDisposable is also added in the internal list.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public IDisposable Add(Action action)
        {
            if (action == null) { throw new ArgumentNullException(nameof(action)); }

            var disposable = new SimpleDisposable(action);
            this.Add(disposable);
            return disposable;
        }

        /// <summary>
        /// Clear the list of disposable. Dispose of ever IDispable in the internal list is called.
        /// </summary>
        public void Clear() => Interlocked.Exchange(ref _disposables, null).ForEach(d => d.Dispose());

        public void Dispose()
        {
            if (!Disposed)
            {
                Clear();
            }
            Disposed = true;
        }

        public IEnumerator<IDisposable> GetEnumerator() => _disposables.ToArray().AsEnumerable().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool IsDisposed => Disposed;

        public bool Disposed { get; set; }
    }
}
