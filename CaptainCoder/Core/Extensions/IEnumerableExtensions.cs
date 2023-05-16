using System;
using System.Collections.Generic;
using System.Linq;
namespace CaptainCoder.Core;

/// <summary>
/// A set of extension methods for <see cref="IEnumerable{T}"/>
/// </summary>
public static class IEnumerableExtensions
{

    /// <summary>
    /// Given an <see cref="IEnumerable{T}"/>, randomizes the order of elements.
    /// </summary>
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> toShuffle) => Shuffle<T>(toShuffle, IRandom.Shared);

    /// <summary>
    /// Given an <see cref="IEnumerable{T}"/>, randomizes the order of elements using the specified <paramref name="randomSource"/>
    /// </summary>
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> toShuffle, IRandom randomSource) => toShuffle.OrderBy(_ => randomSource.NextDouble());

    /// <summary>
    /// Given an <see cref="IEnumerable{T}"/>, peforms the specified <paramref name="action"/> to each element.
    /// </summary>
    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach (T element in enumerable)
        {
            action.Invoke(element);
        }
    }
}