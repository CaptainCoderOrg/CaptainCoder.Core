using System.Collections.Generic;
using System.Linq;
using System;

namespace CaptainCoder.Core;

/// <summary>
/// A <see cref="BagGenerator{T}"/> is a generator produces a sequence of elements by randomly selecting elements
/// from a bag until the bag is empty. Once the bag is empty, it is refilled with the contents of the original bag
/// and reshuffled.
/// </summary>
public class BagGenerator<T> : IGenerator<T>
{
    private readonly T[] _elems;
    private readonly int _copies;
    private readonly Queue<int> _bag;
    private readonly IRandom _rng;

    /// <summary>
    /// Creates a <see cref="BagGenerator{T}"/> specifying the source of randomness, the elements to put in the bag, and
    /// the number of copies of each element to put in the bag.
    /// </summary>
    public BagGenerator(IRandom rng, IEnumerable<T> elements, int copies)
    {
        if (rng == null) { throw new ArgumentNullException("Random number generator must not be null."); }
        if (elements == null) { throw new ArgumentNullException("Bag must not be null."); }
        if (elements.Count() < 1) { throw new ArgumentException("Bag must contain at least one element."); }
        if (copies < 1) { throw new ArgumentException("Generator must contain at least one copy."); }
        _rng = rng;
        _elems = elements.ToArray();
        _copies = copies;
        _bag = new Queue<int>();
    }

    /// <summary>
    /// Creates a <see cref="BagGenerator{T}"/> specifying the elements to put in the bag, and
    /// the number of copies of each element to put in the bag.
    /// </summary>
    public BagGenerator(IEnumerable<T> bag, int copies) : this(IRandom.Shared, bag, copies) {}
    
    /// <summary>
    /// Creates a <see cref="BagGenerator{T}"/> specifying the elements to put in the bag.
    /// </summary>
    public BagGenerator(IEnumerable<T> bag) : this(bag, 1) {}

    /// <inheritdoc/>
    public virtual T Next()
    {
        if (_bag.Count == 0) { FillBag(); }
        return _elems[_bag.Dequeue()];
    }

    private void FillBag() => GenerateOrder().ToList().ForEach(_bag.Enqueue);

    private IEnumerable<int> GenerateOrder() => Enumerable
        .Range(0, _copies * _elems.Length)
        .Select(i => i % _elems.Length)
        .OrderBy(_ => _rng.NextDouble());

}