namespace Finalizers;

public class FinalizerClassTemplate : IDisposable
{
    ~FinalizerClassTemplate()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            // dispose managed resources
        }
    }
}