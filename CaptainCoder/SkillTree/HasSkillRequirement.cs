namespace CaptainCoder.SkillTree;

/// <summary>
/// A <see cref="HasSkillRequirement{E, T}"/> is used to ensure that a <see
/// cref="ISkilledEntity{T}"/> has acquired a specific skill.
/// </summary>
/// <typeparam name="S">The skill type. The type should be useable with a
/// HashSet and Dictionary</typeparam>
/// <typeparam name="E">The entity type.</typeparam>
public class HasSkillRequirement<E, S> : IRequirement<E, S> where S : ISkill where E : ISkilledEntity<S>
{
    /// <summary>
    /// Instantiates an instance for the specified <paramref name="skill"/>.
    /// </summary>
    public HasSkillRequirement(S skill)
    {
        if (skill == null) { throw new ArgumentNullException("Skill is required."); }
        RequiredSkill = skill;
    }
    /// <inheritdoc/>
    public S RequiredSkill { get; }
    /// <inheritdoc/>
    public bool MeetsRequirement(E entity) => entity.Skills.Contains(RequiredSkill);
}