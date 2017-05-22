using System;

namespace Disposables
{
    public class Simple : IIsDisposed
    {
        private Action _action;

        public static Simple Create (Action action)
        {
            return new Simple(action);
        }

        public Simple(Action action)
        {
            _action = action;
        }

        public void Dispose()
        {
            System.Threading.Interlocked.Exchange(ref _action, null)?.Invoke();
        }

        public bool IsDiposed => _action != null;
    }
}
