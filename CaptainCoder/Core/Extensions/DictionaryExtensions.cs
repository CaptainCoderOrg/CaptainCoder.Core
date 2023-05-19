using System.Collections.Generic;
namespace CaptainCoder.Core;

/// <summary></summary>
public static class DictionaryExtensions
{
    /// <summary>
    /// Performs a deep comparison of all key value pairs in <paramref name="dict0"/> and <paramref name="dict1"/>.
    /// </summary>
    public static bool KeyValuePairEquals<K,V>(this IReadOnlyDictionary<K,V> dict0, IReadOnlyDictionary<K,V> dict1)
    {
        if (dict0.Count != dict1.Count) { return false; }
        foreach((K key, V value) in dict0)
        {
            if (!dict1.TryGetValue(key, out V other)) { return false; }
            if (value == null && other == null) { continue; }
            if (value == null || !value.Equals(other)) { return false; }
        }
        return true;
    } 

    /// <summary>
    /// Generates an enumerable of tuples containing each key value pair in the dictionary
    /// </summary>
    public static IEnumerable<(K, V)> ToTuples<K,V>(this IReadOnlyDictionary<K,V> dict)
    {
        foreach ((K key, V value) in dict)
        {
            yield return (key, value);
        }
    }

    /// <summary>
    /// Given an enumerable of key values, creates a dictionary. If a duplicate key is found
    /// this method throws an exception.
    /// </summary>
    public static Dictionary<K, V> ToDictionary<K,V>(this IEnumerable<(K, V)> tuple)
    {
        Dictionary<K,V> dict = new();
        foreach ((K key, V value) in tuple)
        {
            if (dict.ContainsKey(key)) { throw new System.ArgumentException($"The provided enumerator contained a duplicate key: {key}."); }
            dict[key] = value;
        }
        return dict;
    }
}