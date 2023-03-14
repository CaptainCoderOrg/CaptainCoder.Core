namespace CaptainCoder.SkillTree;

/// <summary>
/// Represents a skill
/// </summary>
public interface ISkill
{
    /// <summary>
    /// The name of this skill
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// A description of this skill
    /// </summary>
    public string Description { get; }
}