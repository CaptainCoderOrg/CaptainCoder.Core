namespace CaptainCoder.SkillTree;

/// <summary>
/// Represents a skill within a skill tree.
/// </summary>
/// <typeparam name="T">The type of skill this node holds</typeparam>
public interface ISkillNode<T> where T : ISkill
{
    /// <summary>
    /// A list of requirements that must be met to gain this skill
    /// </summary>
    public IReadOnlyList<IRequirement> Requirements { get; }

    /// <summary>
    /// A list of children skill nodes
    /// </summary>
    IEnumerable<ISkillNode<T>> Children { get; }

    /// <summary>
    /// Checks if the specified <paramref name="character"/> meets the
    /// requirements to acquire this skill.
    /// </summary>
    public bool CheckRequirements(ISkilledEntity<T> character)
    {
        foreach (IRequirement req in Requirements)
        {
            if(!req.MeetsRequirement(character)) { return false; }
        }
        return true;
    }

    /// <summary>
    /// A Requirement acts as a predicate on a character.
    /// </summary>
    public interface IRequirement
    {
        /// <summary>
        /// Checks if the specified <paramref name="character"/> meets this
        /// requirement.
        /// </summary>
        public bool MeetsRequirement(ISkilledEntity<T> character);
    }
}