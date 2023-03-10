using System.Collections.Generic;
using CaptainCoder.Core.Collections;
namespace CaptainCoder.Core;

/// <summary> </summary>
public static class SetExtensionMethods
{
    /// <summary>
    /// Creates and returns a readonly view of the specified <paramref name="set"/>
    /// </summary>
    public static ReadOnlySet<T> AsReadOnly<T>(this ISet<T> set) => new (set);
}