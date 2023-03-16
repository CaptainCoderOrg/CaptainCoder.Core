namespace CaptainCoder.SkillTree;

/// <summary>
/// Represents a skill within a skill tree.
/// </summary>
/// <typeparam name="S">The type of skill this node holds</typeparam>
/// <typeparam name="E">The type of entity that can learn this skill</typeparam>
public interface ISkillNode<E, S> where E : ISkilledEntity<S> where S : ISkill
{
    /// <summary>
    /// The skill that is unlocked with this node.
    /// </summary>
    public S Skill { get; }
    /// <summary>
    /// A list of requirements that must be met to gain this skill
    /// </summary>
    public IReadOnlyList<IRequirement<E, S>> Requirements { get; }

    /// <summary>
    /// A list of children skill nodes
    /// </summary>
    IEnumerable<ISkillNode<E, S>> Children { get; }

    /// <summary>
    /// Checks if the specified <paramref name="entity"/> meets the
    /// requirements to acquire this skill.
    /// </summary>
    public bool CheckRequirements(E entity)
    {
        foreach (IRequirement<E, S> req in Requirements)
        {
            if (!req.MeetsRequirement(entity)) { return false; }
        }
        return true;
    }
}