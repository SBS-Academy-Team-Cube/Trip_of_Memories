using System;
using UnityEngine;

public class CircularQueue<T>
{
    private readonly T[] buffer;
    private int index;
    private int count;

    public CircularQueue(int capacity)
    {
        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than zero.");
        }

        buffer = new T[capacity];
    }

    public void Add(T item)
    {
        buffer[index] = item;
        index = (index + 1) % buffer.Length;
        count = Mathf.Min(count + 1, buffer.Length);
    }

    public void Clear()
    {
        Array.Clear(buffer, 0, buffer.Length);
        index = 0;
        count = 0;
    }

    public T Peek()
    {
        if (count == 0)
        {
            throw new InvalidOperationException("CircularQueue is empty.");
        }

        int oldestIndex = count == buffer.Length ? index : 0;
        return buffer[oldestIndex];
    }
    public int Count => count;
    public int Capacity => buffer.Length;
}
