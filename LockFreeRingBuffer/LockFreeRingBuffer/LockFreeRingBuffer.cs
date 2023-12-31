public class LockFreeRingBuffer<T> where T : class
{
    private readonly T[] _buffer;
    private readonly int _capacity;
    private int _head;
    private int _tail;

    public LockFreeRingBuffer(int capacity)
    {
        _capacity = capacity;
        _buffer = new T[_capacity];
        _head = 0;
        _tail = 0;
    }

    public bool TryWrite(T value)
    {
        do
        {
            var currentTail = _tail;
            var nextTail = (currentTail + 1) % _capacity;

            // Check if the buffer is full
            if (nextTail == Volatile.Read(ref _head))
            {
                return false;
            }

            // Attempt to update the _tail index atomically
            if (Interlocked.CompareExchange(ref _tail, nextTail, currentTail) == currentTail)
            {
                _buffer[currentTail] = value;
                return true;
            }
        }
        while (true);
    }

    public bool TryRead(out T? value)
    {
        do
        {
            var currentHead = _head;
            if (currentHead == Volatile.Read(ref _tail))
            {
                value = default;
                return false;
            }

            // Attempt to update the _head index atomically
            var item = _buffer[currentHead];
            if (Interlocked.CompareExchange(ref _head, (currentHead + 1) % _capacity, currentHead) == currentHead)
            {
                value = item;
                return true;
            }
        }
        while (true);
    }
}