public sealed class AsyncLogger : IAsyncDisposable
{
    private readonly LockFreeRingBuffer<string> _ringBuffer;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly ManualResetEvent _newMessageEvent;
    private readonly Task _logProcessorTask;
    private bool _disposed;

    public AsyncLogger()
    {
        _ringBuffer = new LockFreeRingBuffer<string>(2);
        _cancellationTokenSource = new CancellationTokenSource();
        _newMessageEvent = new ManualResetEvent(false);
        _logProcessorTask = Task.Run(ProcessLogs);
    }

    public void Log(string message)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        while (!_ringBuffer.TryWrite(message))
        {
            // Handle buffer being full, e.g., wait, retry, or drop the message.
        }

        _newMessageEvent.Set();
    }

    private void ProcessLogs()
    {
        while (!_cancellationTokenSource.IsCancellationRequested)
        {
            _newMessageEvent.WaitOne();
            ProcessAllAvailableMessages();
            _newMessageEvent.Reset();
        }

        // Final flush of all messages before exiting
        ProcessAllAvailableMessages();
    }

    private void ProcessAllAvailableMessages()
    {
        while (_ringBuffer.TryRead(out var logMessage))
        {
            // Process the log message
            Console.WriteLine(logMessage);
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _cancellationTokenSource.CancelAsync();
        _newMessageEvent.Set(); // Ensure the log processing task wakes up to process remaining messages
        await _logProcessorTask;
        _cancellationTokenSource.Dispose();
        _newMessageEvent.Close();

        _disposed = true;
    }
}