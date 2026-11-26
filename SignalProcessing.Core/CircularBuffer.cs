using System;

public sealed class CircularBuffer<T>
{
    private readonly T[] _items;
    private int _start;
    private int _count;

    public CircularBuffer(int capacity)
    {
        if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity));
        _items = new T[capacity];
    }

    public int Capacity => _items.Length;
    public int Count => _count;
    public bool IsFull => _count == _items.Length;

    public void Add(T item)
    {
        if (_count < _items.Length)
        {
            _items[(_start + _count) % _items.Length] = item;
            _count++;
        }
        else
        {
            _items[_start] = item;
            _start = (_start + 1) % _items.Length;
        }
    }

    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= _count) throw new ArgumentOutOfRangeException(nameof(index));
            return _items[(_start + index) % _items.Length];
        }
    }

    public T[] ToArray()
    {
        T[] result = new T[_count];
        for (int i = 0; i < _count; i++)
            result[i] = _items[(_start + i) % _items.Length];
        return result;
    }

    public void Clear()
    {
        Array.Clear(_items, 0, _items.Length);
        _start = 0;
        _count = 0;
    }
}