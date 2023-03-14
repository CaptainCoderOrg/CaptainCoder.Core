using System.Collections.ObjectModel;

namespace CaptainCoder.SkillTree;

public class SimpleSkillNode<T> : ISkillNode<T> where T : ISkill
{

    private List<ISkillNode<T>.IRequirement> _requirements = new();
    private List<ISkillNode<T>> _children = new();

    public string Name { get; init; }
    public string Description { get; init; }
    public IReadOnlyList<ISkillNode<T>.IRequirement> Requirements => _requirements.AsReadOnly();

    public IEnumerable<ISkillNode<T>> Children => _children.AsEnumerable();

    public void AddChild(SimpleSkillNode<T> child)
    {
        child._requirements.Add(new HasSkillRequirement { RequiredSkill = this} );
        _children.Add(child);
    }

    public class HasSkillRequirement : ISkillNode<T>.IRequirement
    {
        public ISkillNode<T> RequiredSkill { get; init; }
        public bool MeetsRequirement(ISkilledEntity<T> character) => character.Skills.Contains(RequiredSkill);

    }

}