namespace CaptainCoder.SkillTree;

/// <summary>
/// </summary>
public interface ISkillTree<T> where T : ISkill
{
    /// <summary>
    /// The root node of this skill tree
    /// </summary>
    public ISkillNode<T> Root { get; }
}