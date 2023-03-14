namespace CaptainCoder.SkillTree;

/// <summary>
/// A <see cref="HasSkillRequirement{T}"/> is used to ensure that a <see
/// cref="ISkilledEntity{T}"/> has acquired a specific skill.
/// </summary>
/// <typeparam name="T">The skill type. The type should be useable with a
/// HashSet and Dictionary</typeparam>
public class HasSkillRequirement<T> : IRequirement<T> where T : ISkill
{
    /// <summary>
    /// Instantiates an instance for the specified <paramref name="skill"/>.
    /// </summary>
    public HasSkillRequirement(T skill)
    {
        if (skill == null) { throw new ArgumentNullException("Skill is required."); }
        RequiredSkill = skill;
    }
    /// <inheritdoc/>
    public T RequiredSkill { get; }
    /// <inheritdoc/>
    public bool MeetsRequirement(ISkilledEntity<T> entity) => entity.Skills.Contains(RequiredSkill);
}