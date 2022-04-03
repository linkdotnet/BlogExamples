using System.Buffers;

namespace LinkDotNet;

public ref struct ValueStringBuilder
{
    private int _bufferPosition;
    private Span<char> _buffer;

    public ValueStringBuilder()
    {
        _bufferPosition = 0;
        _buffer = new char[32];
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

    private void Grow(int capacity = 0)
    {
        var currentSize = _buffer.Length;
        var newSize = capacity > 0 ? capacity : currentSize * 2;
        var rented = ArrayPool<char>.Shared.Rent(newSize);
        _buffer.CopyTo(rented);
        _buffer = rented;
        ArrayPool<char>.Shared.Return(rented);
    }
}