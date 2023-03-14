namespace CaptainCoder.SkillTree;

/// <summary>
/// </summary>
public interface ISkillTree<T> where T : ISkill
{
    /// <summary>
    /// The root node of this skill tree
    /// </summary>
    public ISkillNode<T> Root { get; }

    /// <summary>
    /// Retrieves the <see cref="ISkillNode{T}"/> associated with the specified
    /// <paramref name="skill"/>. Throws an exception if the specified <paramref
    /// name="skill"/> is not in this <see cref="ISkillTree{T}"/>.
    /// </summary>
    public ISkillNode<T> GetNode(T skill);
}