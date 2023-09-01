using System;

public class ChunkedList<T>
{
    private const int ChunkSize = 8000;
    private T[][] _chunks;

    public ChunkedList()
    {
        _chunks = new T[1][];
        _chunks[0] = new T[ChunkSize];
        Count = 0;
    }

    public T this[int index]
    {
        get
        {
            ArgumentOutOfRangeException.ThrowIfNegative(index);
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Count);

            var chunkIndex = index / ChunkSize;
            var innerIndex = index % ChunkSize;
            return _chunks[chunkIndex][innerIndex];
        }
        set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(index);
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Count);

            var chunkIndex = index / ChunkSize;
            var innerIndex = index % ChunkSize;
            _chunks[chunkIndex][innerIndex] = value;
        }
    }

    public void Add(T item)
    {
        if (Count == _chunks.Length * ChunkSize)
        {
            // Create a new larger set of chunks
            var newChunks = new T[_chunks.Length + 1][];
            Array.Copy(_chunks, newChunks, _chunks.Length);
            newChunks[_chunks.Length] = new T[ChunkSize];
            _chunks = newChunks;
        }

        var addToChunk = Count / ChunkSize;
        var addToIndex = Count % ChunkSize;
        _chunks[addToChunk][addToIndex] = item;

        Count++;
    }

    public int Count { get; private set; }

    public void Clear()
    {
        Count = 0;
    }
}