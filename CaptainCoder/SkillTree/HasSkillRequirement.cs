namespace CaptainCoder.SkillTree;
public class HasSkillRequirement<T> : IRequirement<T> where T : ISkill
{
    public HasSkillRequirement(T skill)
    {
        if (skill == null) { throw new ArgumentNullException("Skill is required."); }
        RequiredSkill = skill;
    }
    public T RequiredSkill { get; }
    public bool MeetsRequirement(ISkilledEntity<T> entity) => entity.Skills.Contains(RequiredSkill);
}