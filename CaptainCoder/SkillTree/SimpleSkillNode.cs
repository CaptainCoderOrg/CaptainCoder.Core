namespace CaptainCoder.SkillTree;

public class SimpleSkillNode<T> : ISkillNode<T> where T : ISkill
{
    private List<IRequirement<T>> _requirements = new();
    private List<ISkillNode<T>> _children = new();

    public SimpleSkillNode(T skill)
    {
        if (skill == null) { throw new ArgumentNullException("Skill is required."); }
        Skill = skill;
    }

    public T Skill { get; }
    public IReadOnlyList<IRequirement<T>> Requirements => _requirements.AsReadOnly();
    public IEnumerable<ISkillNode<T>> Children => _children.AsEnumerable();

    public void AddChild(SimpleSkillNode<T> child)
    {
        child._requirements.Add(new HasSkillRequirement<T>(Skill));
        _children.Add(child);
    }


}