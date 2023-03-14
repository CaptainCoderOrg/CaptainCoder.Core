namespace CaptainCoder.SkillTree;

/// <summary>
/// Represents a skill within a skill tree.
/// </summary>
/// <typeparam name="T">The type of skill this node holds</typeparam>
public interface ISkillNode<T> where T : ISkill
{
    /// <summary>
    /// The skill that is unlocked with this node.
    /// </summary>
    public T Skill { get; }
    /// <summary>
    /// A list of requirements that must be met to gain this skill
    /// </summary>
    public IReadOnlyList<IRequirement<T>> Requirements { get; }

    /// <summary>
    /// A list of children skill nodes
    /// </summary>
    IEnumerable<ISkillNode<T>> Children { get; }

    /// <summary>
    /// Checks if the specified <paramref name="entity"/> meets the
    /// requirements to acquire this skill.
    /// </summary>
    public bool CheckRequirements(ISkilledEntity<T> entity)
    {
        foreach (IRequirement<T> req in Requirements)
        {
            if(!req.MeetsRequirement(entity)) { return false; }
        }
        return true;
    }
}