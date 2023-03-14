namespace CaptainCoder.SkillTree;

/// <summary>
/// An <see cref="ISkilledEntity{T}"/> represents an entity that can acquire
/// skills of the specified type.
/// </summary>
public interface ISkilledEntity<T> where T : ISkill
{
    /// <summary>
    /// A HashSet of skills that this entity has acquired
    /// </summary>
    public HashSet<ISkillNode<T>> Skills { get; }
}