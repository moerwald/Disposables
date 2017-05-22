using System;

namespace Disposables
{
    public class EmptyDisposable : IDisposable
    {
        public void Dispose()
        {
            // Nothing to do
        }
    }
}
