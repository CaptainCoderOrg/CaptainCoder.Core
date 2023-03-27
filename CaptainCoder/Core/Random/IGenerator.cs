using System.Collections.Generic;
namespace CaptainCoder.Core;

/// <summary>
/// An <see cref="IGenerator{T}"/> is capable of infinitely producing elements of a specified type.
/// </summary>
public interface IGenerator<T>
{
    /// <summary>
    /// Returns the next element in the generator.
    /// </summary>
    public T Next();
}

/// <summary>
/// Provides helper methods for the IGenerator interface
/// </summary>
public static class IGeneratorExtensions
{
    /// <summary>
    /// Generates an infinite stream of elements.
    /// </summary>
    public static IEnumerator<T> Stream<T>(this IGenerator<T> gen)
    {
        while (true)
        {
            yield return gen.Next();
        }
    }
}