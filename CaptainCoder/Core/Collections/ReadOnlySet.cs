using System;
using System.Collections;
using System.Collections.Generic;
namespace CaptainCoder.Core.Collections;

/// <summary>
/// A read only view of a set.
/// </summary>
public class ReadOnlySet<T> : IReadOnlyCollection<T>, ISet<T>
{
    private readonly ISet<T> _set;

    /// <summary>
    /// Instantiates a view of the given set.
    /// </summary>
    /// <param name="set"></param>
    public ReadOnlySet(ISet<T> set) => _set = set;

    /// <inheritdoc/>
    public int Count => _set.Count;
    /// <inheritdoc/>
    public bool IsReadOnly => true;
    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator() => _set.GetEnumerator();
    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_set).GetEnumerator();
    /// <inheritdoc/>
    public bool IsSubsetOf(IEnumerable<T> other) => _set.IsSubsetOf(other);
    /// <inheritdoc/>
    public bool IsSupersetOf(IEnumerable<T> other) => _set.IsSupersetOf(other);
    /// <inheritdoc/>
    public bool IsProperSupersetOf(IEnumerable<T> other) => _set.IsProperSupersetOf(other);
    /// <inheritdoc/>
    public bool IsProperSubsetOf(IEnumerable<T> other) => _set.IsProperSubsetOf(other);
    /// <inheritdoc/>
    public bool Overlaps(IEnumerable<T> other) => _set.Overlaps(other);
    /// <inheritdoc/>
    public bool SetEquals(IEnumerable<T> other) => _set.SetEquals(other);
    /// <inheritdoc/>
    public bool Contains(T item) => _set.Contains(item);
    /// <inheritdoc/>    
    public void CopyTo(T[] array, int arrayIndex) => _set.CopyTo(array, arrayIndex);
    /// <inheritdoc/>  
    public bool Remove(T item) => throw new NotSupportedException("Set is a read only set.");

    #region Unsupported methods
    /// <summary>
    /// This method is not supported.
    /// </summary>
    void ICollection<T>.Add(T item) => throw new NotSupportedException("Set is a read only set.");
    /// <summary>
    /// This method is not supported.
    /// </summary>
    void ISet<T>.UnionWith(IEnumerable<T> other) => throw new NotSupportedException("Set is a read only set.");
    /// <summary>
    /// This method is not supported.
    /// </summary>
    void ISet<T>.IntersectWith(IEnumerable<T> other) => throw new NotSupportedException("Set is a read only set.");
    /// <summary>
    /// This method is not supported.
    /// </summary>
    void ISet<T>.ExceptWith(IEnumerable<T> other) => throw new NotSupportedException("Set is a read only set.");
    /// <summary>
    /// This method is not supported.
    /// </summary>
    void ISet<T>.SymmetricExceptWith(IEnumerable<T> other) => throw new NotSupportedException("Set is a read only set.");
    /// <summary>
    /// This method is not supported.
    /// </summary>
    bool ISet<T>.Add(T item) => throw new NotSupportedException("Set is a read only set.");
    /// <summary>
    /// This method is not supported.
    /// </summary>
    void ICollection<T>.Clear() => throw new NotImplementedException();
    #endregion
}