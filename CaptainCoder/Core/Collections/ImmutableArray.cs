using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace CaptainCoder.Core.Collections;

/// <summary>
/// An <see cref="ImmutableArray{T}"/> is a wrapper for an array that is immutable
/// and implements a "nice" Equal check and ToString. This class should not be used as
/// a hashable key.
/// </summary>
public class ImmutableArray<T> : IEnumerable<T>
{
    private readonly T[] _array;

    /// <summary>
    /// Constructs an <see cref="ImmutableArray{T}"/> with the specified values.
    /// </summary>
    public ImmutableArray(IEnumerable<T> values)
    {
        _array = values.ToArray();
    }

    /// <summary>
    /// Accesses the elements in the underlying array.
    /// </summary>
    /// <value></value>
    public T this[int index]
    {
        get => _array[index];
    }

    /// <summary>
    /// Retrieves the length of the underlying array.
    /// </summary>
    public int Length => _array.Length;

    /// <summary>
    /// Creates a new array containing the same elements as this array.
    /// </summary>
    public T[] ToArray() => _array.ToArray();

    /// <summary>
    /// Checks the structural equality of the underlying arrays.
    /// </summary>
    public override bool Equals(object obj)
    {
        if (obj == this) { return true; }
        if (obj is not ImmutableArray<T> asImmutableArray) { return false; }
        if (asImmutableArray.Length != this.Length) { return false; }
        if (asImmutableArray._array == this._array) { return true; }
        return _array.SequenceEqual(asImmutableArray._array);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return System.HashCode.Combine(_array, Length);
    }

    /// <inheritdoc />
    public override string ToString() => $"ImmutableArray: [ {string.Join(',', _array)} ]";

    /// <inheritdoc />
    public IEnumerator<T> GetEnumerator()
    {
        foreach (T el in _array) { yield return el; }
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
    {
        foreach (T el in _array) { yield return el; }
    }


}