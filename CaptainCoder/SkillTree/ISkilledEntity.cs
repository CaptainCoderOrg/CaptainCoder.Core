namespace CaptainCoder.SkillTree;

/// <summary>
/// An <see cref="ISkilledEntity{T}"/> represents an entity that can acquire
/// skills of the specified type.
/// </summary>
public interface ISkilledEntity<T>
{
    /// <summary>
    /// A HashSet of skills that this entity has acquired
    /// </summary>
    public HashSet<T> Skills { get; }
}