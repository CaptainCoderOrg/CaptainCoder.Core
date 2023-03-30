using System.Collections.Generic;
namespace CaptainCoder.Dice;

/// <summary>
/// An <see cref="IRollContext"/> is used to look up and evaluate variables within a <see cref="DiceNotation"/>.
/// </summary>
public interface IRollContext
{
    /// <summary>
    /// Looks up the value of the specified identifier.
    /// </summary>
    public int Lookup(string id);
}

/// <summary>
/// This class provides helper methods for easily looking creating an <see cref="IRollContext"/> using a dictionary
/// </summary>
public static class IRollContextExtensions
{
    /// <summary>
    /// For convenience, this method creates a view of a dictionary. If the dictionary changes, the underlying context
    /// will use the new values.
    /// </summary>
    public static IRollContext ToRollContext(this Dictionary<string, int> dict) => new DictionaryBackedContext(dict);
}

internal class DictionaryBackedContext : IRollContext
{
    private readonly Dictionary<string, int> _context;
    public DictionaryBackedContext(Dictionary<string, int> context) => _context = context;
    public int Lookup(string id) => _context[id];
}