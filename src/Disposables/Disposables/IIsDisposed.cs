namespace Disposables
{
    public interface IIsDisposed : System.IDisposable
    {
        bool IsDisposed { get; }
    }
}
