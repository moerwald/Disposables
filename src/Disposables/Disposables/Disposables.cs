using System;

namespace Disposables
{
    public static class Disposables
    {
        public static IDisposable Empty => new EmptyDisposable();
    }
}
