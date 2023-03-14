namespace CaptainCoder.SkillTree;

public class SkillTreeBuilder<T> where T : ISkill
{
    private T _root;
    public SkillTreeBuilder(T root)
    {
        _root = root;
    }

    // Doesn't make sense to allow
    public SkillTreeBuilder<T> AddSkill(T parent, T child)
    {
        return this;
    }

    public SkillTreeBuilder<T> AddRequirement(T skill, IRequirement<T> requirement)
    {
        return this;
    }

    public ISkillTree<T> Build()
    {
        return null;
    }
}