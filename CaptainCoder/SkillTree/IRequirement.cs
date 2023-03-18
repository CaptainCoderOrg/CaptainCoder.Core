namespace CaptainCoder.SkillTree;

/// <summary>
/// A Requirement acts as a predicate on a character.
/// </summary>
public interface IRequirement<E, S> where E : ISkilledEntity<S>
{
    /// <summary>
    /// Checks if the specified <paramref name="entity"/> meets this
    /// requirement.
    /// </summary>
    public bool MeetsRequirement(E entity);
}