using System.Collections.Generic;
namespace CaptainCoder.Core;

/// <summary></summary>
public static class DictionaryExtensions
{
    /// <summary>
    /// Performs a deep comparison of all key value pairs in <paramref name="dict0"/> and <paramref name="dict1"/>.
    /// </summary>
    public static bool KeyValuePairEquals<K,V>(this Dictionary<K,V> dict0, Dictionary<K,V> dict1)
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
    public static IEnumerable<(K, V)> ToTuples<K,V>(this Dictionary<K,V> dict)
    {
        foreach ((K key, V value) in dict)
        {
            yield return (key, value);
        }
    }
}