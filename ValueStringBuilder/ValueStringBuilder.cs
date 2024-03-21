using System.Buffers;

namespace LinkDotNet;

public ref struct ValueStringBuilder
{
    private int _bufferPosition;
    private Span<char> _buffer;
    private char[]? arrayFromPool;

    public ValueStringBuilder()
    {
        _bufferPosition = 0;
        _buffer = new char[32];
        arrayFromPool = null;
    }

    public ref char this[int index] => ref _buffer[index];

    public void Append(char c)
    {
        if (_bufferPosition == _buffer.Length - 1)
        {
            Grow();
        }

        _buffer[_bufferPosition++] = c;
    }

    public void Append(ReadOnlySpan<char> str)
    {
        var newSize = str.Length + _bufferPosition;
        if (newSize > _buffer.Length)
            Grow(newSize * 2);

        str.CopyTo(_buffer[_bufferPosition..]);
        _bufferPosition += str.Length;
    }

    public void AppendLine(ReadOnlySpan<char> str)
    {
        Append(str);
        Append(Environment.NewLine);
    }

    public override string ToString() => new(_buffer[.._bufferPosition]);

    public void Dispose()
    {
        if (arrayFromPool is not null)
        {
            ArrayPool<char>.Shared.Return(arrayFromPool);
        }
    }

    private void Grow(int capacity = 0)
    {
        var currentSize = _buffer.Length;
        var newSize = capacity > 0 ? capacity : currentSize * 2;
        var rented = ArrayPool<char>.Shared.Rent(newSize);
        var oldBuffer = arrayFromPool;
        _buffer.CopyTo(rented);
        _buffer = arrayFromPool = rented;
        if (oldBuffer is not null)
        {
            ArrayPool<char>.Shared.Return(oldBuffer);
        }
    }
}