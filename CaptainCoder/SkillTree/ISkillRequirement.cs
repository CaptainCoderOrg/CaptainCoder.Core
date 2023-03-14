namespace CaptainCoder.SkillTree;

/// <summary>
/// A Requirement acts as a predicate on a character.
/// </summary>
public interface IRequirement<T> where T : ISkill
{
    /// <summary>
    /// Checks if the specified <paramref name="entity"/> meets this
    /// requirement.
    /// </summary>
    public bool MeetsRequirement(ISkilledEntity<T> entity);
}